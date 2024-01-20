using RoleBasedMenuSystem.Core.Entities.Concrete;

namespace RoleBasedMenuSystem.Infrastructure.Abstract.Query;

public interface IUserQuery
{
    Task<List<Guid>> GetRoleIdByUserIdAsync(Guid id);
    Task<List<(Guid id, string name)>> GetAllRoles();
    Task<AppUser> GetUserByUserNameAsync(string userName);
}