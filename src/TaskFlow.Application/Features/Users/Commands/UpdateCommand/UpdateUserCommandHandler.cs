using FluentValidation.Results;
using MediatR;
using TaskFlow.Domain.Identity;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Users.Commands.UpdateCommand;

public class UpdateUserCommandHandler(
    ICurrentUser currentUser,
    IUserWriteOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var loggedUser1 = await currentUser.GetCurrentUser();

        await Validate(request, loggedUser1.Email);

        var user = await repository.GetById(loggedUser1.Id);

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