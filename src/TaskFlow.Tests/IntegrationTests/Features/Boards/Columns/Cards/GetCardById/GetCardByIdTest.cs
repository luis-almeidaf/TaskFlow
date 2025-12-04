using System.Net;
using System.Text.Json;
using FluentAssertions;
using TaskFlow.Exception;

namespace TaskFlow.Tests.IntegrationTests.Features.Boards.Columns.Cards.GetCardById;

public class GetCardByIdTest : TaskFlowClassFixture
{
    private const string Route = "Boards";

    private readonly Guid _boardId;
    private readonly Guid _columnId;
    private readonly Guid _cardId;
    private readonly string _boardOwnerToken;

    public GetCardByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _boardId = webApplicationFactory.Board.GetId();
        _columnId = webApplicationFactory.Column.GetId();
        _cardId = webApplicationFactory.Card.GetId();
        _boardOwnerToken = webApplicationFactory.UserWithBoards.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet($"/{Route}/{_boardId}/columns/{_columnId}/cards/{_cardId}", _boardOwnerToken);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetGuid().Should().Be(_cardId);
        responseData.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task Error_Card_Not_Found()
    {
        var fakeId = Guid.NewGuid();
        
        var response = await DoGet($"/{Route}/{_boardId}/columns/{_columnId}/cards/{fakeId}", _boardOwnerToken);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors =responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        
        var expectedMessage = ResourceErrorMessages.CARD_NOT_FOUND;

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}