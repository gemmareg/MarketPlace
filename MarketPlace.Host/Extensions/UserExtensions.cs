using System.Security.Claims;

namespace MarketPlace.Host.Extensions
{
    public static class UserExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user?.FindFirst("sub")?.Value
                ?? user?.FindFirst("id")?.Value ?? string.Empty;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user?.IsInRole("Admin") ?? false;
        }
    }
}
