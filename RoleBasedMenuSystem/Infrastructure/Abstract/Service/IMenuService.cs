using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Model.DataTransferObjects.Menu;

namespace RoleBasedMenuSystem.Infrastructure.Abstract.Service;

public interface IMenuService
{
    Task<DataResult> GetAllAsync();
    Task<DataResult> CreateMenuAsync(CreateMenuRequest request);
    Task<DataResult> UpdateMenuAsync(UpdateMenuRequest request);
    Task<DataResult> DeleteMenuAsync(Guid id);
    Task<DataResult> GetAllMenuByPermissionAsync();
}