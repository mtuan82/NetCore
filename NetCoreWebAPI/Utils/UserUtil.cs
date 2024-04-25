using System.Security.Claims;

namespace NetCoreWebAPI.Utils
{
    public static class UserUtil
    {
        public static string GetUserId(ClaimsPrincipal user)
        {
            string? userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId ?? "";
        }

        public static string GetUserName(ClaimsPrincipal user)
        {
            string? userId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return userId ?? "";
        }
    }
}
