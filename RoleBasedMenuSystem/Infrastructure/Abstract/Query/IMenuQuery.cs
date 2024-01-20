using RoleBasedMenuSystem.Model.DataTransferObjects.Menu;

namespace RoleBasedMenuSystem.Infrastructure.Abstract.Query;

public interface IMenuQuery
{
    Task<List<GetAllMenuByRoleResponse>> GetAllByRoleAsync(Guid userId);
}