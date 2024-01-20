using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RoleBasedMenuSystem.Core.Security.JWT
{
    
    public static class ClaimExtensions
    {
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }
        
        public static void AddUniqueName(this ICollection<Claim> claims, string username)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, username));
        }

        public static void AddPhoneNumber(this ICollection<Claim> claims, string phoneNumber)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Prn, phoneNumber));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, Guid[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(JwtRegisteredClaimNames.Sub, role.ToString())));
        }
    }
}
