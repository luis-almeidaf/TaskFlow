using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Security.Cryptography;
using TaskFlow.Domain.Security.Tokens;
using TaskFlow.Infrastructure.DataAccess;
using TaskFlow.Tests.Builders.Entities;
using TaskFlow.Tests.IntegrationTests.Resources;

namespace TaskFlow.Tests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"InMemoryDb_{Guid.NewGuid()}";

    public BoardIdentityManager Board { get; private set; } = null!;
    public ColumnIdentityManager Column { get; private set; } = null!;
    public CardIdentityManager Card { get; private set; } = null!;
    public UserIdentityManager UserOwner { get; private set; } = null!;
    public UserIdentityManager UserAdmin { get; private set; } = null!;
    public UserIdentityManager UserGuest { get; private set; } = null!;
    public UserIdentityManager UserOutOfBoard { get; private set; } = null!;
    public RefreshTokenManager RefreshTokenValid { get; private set; } = null!;
    public RefreshTokenManager RefreshTokenExpired { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test").ConfigureServices(services =>
        {
            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<TaskFlowDbContext>(config =>
            {
                config.UseInMemoryDatabase(_databaseName);
                config.UseInternalServiceProvider(provider);
            });
            var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();
            var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();
            var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
            StartDatabase(dbContext, passwordEncrypter, accessTokenGenerator);
        });
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
    }

    private void StartDatabase(
        TaskFlowDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator tokenGenerator)
    {
        var userOwner = AddUser(dbContext, passwordEncrypter);
        var userAdmin = AddUser(dbContext, passwordEncrypter);
        var userGuest = AddUser(dbContext, passwordEncrypter);
        var userOutOfBoard = AddUser(dbContext, passwordEncrypter);

        var expiredRefreshToken = AddRefreshToken(dbContext, userGuest, "tokenExpired", DateTime.UtcNow.AddDays(-10));
        var validRefreshToken = AddRefreshToken(dbContext, userOutOfBoard, "validToken", DateTime.UtcNow.AddDays(7));

        RefreshTokenValid = new RefreshTokenManager(validRefreshToken.Token);
        RefreshTokenExpired = new RefreshTokenManager(expiredRefreshToken.Token);

        UserOwner = new UserIdentityManager(userOwner, "B!1qwerty", tokenGenerator.Generate(userOwner));
        UserAdmin = new UserIdentityManager(userAdmin, "B!1qwerty", tokenGenerator.Generate(userAdmin));
        UserGuest = new UserIdentityManager(userGuest, "B!1qwerty", tokenGenerator.Generate(userGuest));
        UserOutOfBoard = new UserIdentityManager(userOutOfBoard, "B!1qwerty", tokenGenerator.Generate(userOutOfBoard));

        var board = AddBoard(dbContext, userOwner);

        AddBoardMember(dbContext, userOwner, board, BoardRole.Owner);
        AddBoardMember(dbContext, userAdmin, board, BoardRole.Admin);
        AddBoardMember(dbContext, userGuest, board, BoardRole.Guest);

        var column = AddColumn(dbContext, board);

        AddCard(dbContext, column);
        AddCard(dbContext, column);
        AddCard(dbContext, column);

        dbContext.SaveChanges();
    }

    private static User AddUser(TaskFlowDbContext dbContext, IPasswordEncrypter passwordEncrypter)
    {
        var user = UserBuilder.Build();
        user.Password = "B!1qwerty";

        user.Password = passwordEncrypter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        return user;
    }

    private static RefreshToken AddRefreshToken(TaskFlowDbContext dbContext, User user, string token,
        DateTime expiresOnUtc)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            ExpiresOnUtc = expiresOnUtc,
            UserId = user.Id
        };

        dbContext.RefreshTokens.Add(refreshToken);

        return refreshToken;
    }

    private static void AddBoardMember(TaskFlowDbContext dbContext, User user, Board board, BoardRole boardRole)
    {
        var boardMember = BoardMemberBuilder.Build(user, board);
        boardMember.Role = boardRole;

        dbContext.BoardMembers.Add(boardMember);
    }

    private Board AddBoard(TaskFlowDbContext dbContext, User user)
    {
        var board = BoardBuilder.Build(user);

        dbContext.Boards.Add(board);

        Board = new BoardIdentityManager(board);

        return board;
    }

    private Column AddColumn(TaskFlowDbContext dbContext, Board board)
    {
        var column = ColumnBuilder.Build(board);

        dbContext.Columns.Add(column);

        Column = new ColumnIdentityManager(column);

        return column;
    }

    private void AddCard(TaskFlowDbContext dbContext, Column column)
    {
        var card = CardBuilder.Build(column);

        dbContext.Cards.Add(card);

        Card = new CardIdentityManager(card);
    }
}