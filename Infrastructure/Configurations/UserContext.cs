using Application.Configurations;

namespace Infrastructure.Configurations;

public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid UserId => Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value, out Guid parsedId) ? parsedId : throw new ApplicationException();

    public string Username => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ApplicationException();
}
