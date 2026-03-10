using MarketPlace.Host.Abstractions.Security;
using System.Security.Claims;

namespace MarketPlace.Host.Security
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? UserId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? Name => User?.Identity?.Name;

        public IEnumerable<string> Roles =>
            User?.FindAll(ClaimTypes.Role).Select(x => x.Value)
            ?? Enumerable.Empty<string>();
    }
}
