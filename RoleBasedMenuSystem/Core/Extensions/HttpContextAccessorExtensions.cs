using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;

namespace RoleBasedMenuSystem.Core.Extensions;


 public static class HttpContextAccessorExtensions
    {
        public static (bool login, string text) LoginExists(this IHttpContextAccessor httpContextAccessor)
        {
            if (string.IsNullOrEmpty(httpContextAccessor.AccessToken().userId))
                return (false, "Giriş yapılmadı");

            return (true, string.Empty);
        }

        public static (string accessToken, string userId, List<Claim> claims, List<string> roles) AccessToken(this IHttpContextAccessor httpContextAccessor)
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            HttpRequest request = httpContext?.Request;

            string accessToken = request?.Headers["Authorization"].FirstOrDefault();
            string idToken = request?.Headers["IdToken"].FirstOrDefault();

            if (string.IsNullOrEmpty(accessToken))
                if (request.Query.TryGetValue("access_token", out StringValues token))
                    accessToken = $"Bearer {token}";

            string userId = "";
            List<string> roles = new();
            List<Claim> claims = null;
            if (!string.IsNullOrEmpty(accessToken))
            {
                if (accessToken.StartsWith("Bearer")) accessToken = accessToken.Replace("Bearer ", "").Replace("\"", "").Trim();

                JwtSecurityToken securityToken = new JwtSecurityToken(accessToken);

                if (securityToken.Claims.Any())
                {
                    List<Claim> claim = securityToken.Claims.ToList();
                    userId = claim.FirstOrDefault()!.Value;
                    // roles = claim[3].Value;
                }
            }

            if (string.IsNullOrEmpty(idToken)) return (accessToken, userId, claims, roles);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(idToken)) claims = tokenHandler.ReadJwtToken(idToken).Claims.ToList();

            return (accessToken, userId, claims, roles);
        }
    }