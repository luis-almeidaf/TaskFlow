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
    public BoardIdentityManager Board { get; private set; } = null!;
    public ColumnIdentityManager Column { get; private set; } = null!;
    public CardIdentityManager Card { get; private set; } = null!;
    public UserIdentityManager User { get; private set; } = null!;
    public UserIdentityManager UserWithBoards { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test").ConfigureServices(services =>
        {
            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<TaskFlowDbContext>(config =>
            {
                config.UseInMemoryDatabase("InMemoryDbForTesting");
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
        AddUser(dbContext, passwordEncrypter, tokenGenerator);

        var userWithBoards = AddUserWithBoards(dbContext, passwordEncrypter, tokenGenerator);

        var board = AddBoard(dbContext, userWithBoards);
        
        var column = AddColumn(dbContext, board);

        AddCard(dbContext, column);
        AddCard(dbContext, column);
        AddCard(dbContext, column);
        
        dbContext.SaveChanges();
    }

    private void AddUser(
        TaskFlowDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();

        var password = user.Password;

        user.Password = passwordEncrypter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User = new UserIdentityManager(user, password, token);
    }
    
    private User AddUserWithBoards(
        TaskFlowDbContext dbContext,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator tokenGenerator)
    {
        var userWithBoards = UserBuilder.Build();

        var password = userWithBoards.Password;

        userWithBoards.Password = passwordEncrypter.Encrypt(userWithBoards.Password);

        dbContext.Users.Add(userWithBoards);

        var board = BoardBuilder.Build(userWithBoards);

        dbContext.Boards.Add(board);

        var token = tokenGenerator.Generate(userWithBoards);

        UserWithBoards = new UserIdentityManager(userWithBoards, password, token);

        return userWithBoards;
    }

    private Board AddBoard(TaskFlowDbContext dbContext, User user)
    {
        var board = BoardBuilder.Build(user);
        board.Users.Add(user);

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