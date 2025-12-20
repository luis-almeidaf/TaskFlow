using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Exception.ExceptionsBase;

namespace TaskFlow.Application.Features.Boards.Members.Commands.AddBoardMemberCommand;

public class AddBoardMemberCommandHandler(
    IUnitOfWork unitOfWork,
    IBoardWriteOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository) : IRequestHandler<AddBoardMemberCommand, AddBoardMemberResponse>
{
    public async Task<AddBoardMemberResponse> Handle(AddBoardMemberCommand request, CancellationToken cancellationToken)
    {
        Validate(request);

        var board = await repository.GetById(request.BoardId);
        if (board is null) throw new BoardNotFoundException();

        var userToAdd = await userReadOnlyRepository.GetUserByEmail(request.UserEmail);
        if (userToAdd is null) throw new UserNotFoundException();

        if (board.Members.Any(bm => bm.UserId == userToAdd.Id)) throw new UserAlreadyInBoardException();

        var newBoardMember = new BoardMember
        {
            Id = Guid.NewGuid(),
            BoardId = board.Id,
            Role = request.Role,
            UserId = userToAdd.Id
        };

        repository.AddBoardMember(newBoardMember);

        await unitOfWork.Commit();

        return new AddBoardMemberResponse
        {
            Name = userToAdd.Name,
            Email = userToAdd.Email
        };
    }

    private static void Validate(AddBoardMemberCommand request)
    {
        var result = new AddBoardMemberValidator().Validate(request);

        if (result.IsValid) return;
        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}