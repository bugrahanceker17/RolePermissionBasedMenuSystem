using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RoleBasedMenuSystem.Core.Entities.Concrete;
using RoleBasedMenuSystem.Core.Security.Encryption;

namespace RoleBasedMenuSystem.Core.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        private IConfiguration Configuration { get; }
        private readonly TokenOptionsModel _tokenOptions;
        private readonly DateTime _accessTokenExpiration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptionsModel>();
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        }

        public AccessToken CreateToken(AppUser user, List<Guid> role)
        {
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, role);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptionsModel tokenOptions, AppUser user, SigningCredentials signingCredentials, List<Guid> role)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, role),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private static IEnumerable<Claim> SetClaims(AppUser user, List<Guid> identityRole)
        {
            List<Claim> claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            // claims.AddEmail(user.Id);
            claims.AddPhoneNumber(user.PhoneNumber ?? string.Empty);
            claims.AddUniqueName(user.UserName ?? string.Empty);
            claims.AddRoles(identityRole.ToArray());

            return claims;
        }
    }
}