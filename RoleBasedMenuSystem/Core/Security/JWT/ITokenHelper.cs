using Microsoft.AspNetCore.Identity;
using RoleBasedMenuSystem.Core.Entities.Concrete;

namespace RoleBasedMenuSystem.Core.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(AppUser user, List<Guid> role); 
    }
}

