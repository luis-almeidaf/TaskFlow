using System.Net.Http.Json;

namespace WebApi.Test;

public class TaskFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public TaskFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    protected async Task<HttpResponseMessage> DoPost(string requestUri, object request)
    {
        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }
}