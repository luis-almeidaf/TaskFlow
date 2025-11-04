using TaskFlow.Domain.Security.Tokens;

namespace TaskFlow.Api.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

    public string TokenOnRequest()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization["Bearer ".Length..].Trim();
    }
}