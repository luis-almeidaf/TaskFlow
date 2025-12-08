using FluentAssertions;
using TaskFlow.Application.Features.Boards.Commands.CreateBoardCommand;
using TaskFlow.Domain.Entities;
using TaskFlow.Exception;
using TaskFlow.Exception.ExceptionsBase;
using TaskFlow.Tests.Builders.Commands.Boards;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.Builders.LoggedUser;
using TaskFlow.Tests.Builders.Repositories;

namespace TaskFlow.Tests.UnitTests.Features.Boards.Commands.Create;

public class CreateBoardCommandHandlerTest
{
    [Fact]
    public async Task Success()
    {
        var request = CreateBoardCommandBuilder.Build();

        var user = UserBuilder.Build();

        var handler = CreateHandler(user);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = CreateBoardCommandBuilder.Build();
        request.Name = string.Empty;

        var user = UserBuilder.Build();

        var handler = CreateHandler(user);

        var act = async () => await handler.Handle(request, CancellationToken.None);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }

    private CreateBoardCommandHandler CreateHandler(User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new BoardWriteOnlyRepositoryBuilder().Build();

        return new CreateBoardCommandHandler(unitOfWork, loggedUser, repository);
    }
}