using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Final.Modules
{
    public static class HttpContextExtensions
    {
        public static async Task UpdateClaimAsync(this HttpContext httpContext, string claimType, string claimValue)
        {
            var user = httpContext.User as ClaimsPrincipal;
            var identity = user?.Identity as ClaimsIdentity;

            // Tìm và xóa claim cũ (nếu tồn tại)
            var existingClaim = identity?.FindFirst(claimType);
            if (existingClaim != null)
            {
                identity.RemoveClaim(existingClaim);
            }

            // Thêm claim mới
            identity?.AddClaim(new Claim(claimType, claimValue));

            // Cập nhật HttpContext
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
        }
    }
}
