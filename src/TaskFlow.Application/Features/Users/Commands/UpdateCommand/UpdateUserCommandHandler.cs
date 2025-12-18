using FluentValidation.Results;
using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.UpdateCommand;

public class UpdateUserCommandHandler(
    IUserRetriever userRetriever,
    IUserWriteOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await userRetriever.GetCurrentUser();

        await Validate(request, currentUser.Email);

        var user = await repository.GetById(currentUser.Id);

        user!.Name = request.Name;
        user.Email = request.Email;

        repository.Update(user);

        await unitOfWork.Commit();

        return Unit.Value;
    }

    private async Task Validate(UpdateUserCommand request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = await validator.ValidateAsync(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userExist = await userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (userExist)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}