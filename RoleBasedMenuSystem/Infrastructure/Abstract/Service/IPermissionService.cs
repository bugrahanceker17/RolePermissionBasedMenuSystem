using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Model.DataTransferObjects.Permission;

namespace RoleBasedMenuSystem.Infrastructure.Abstract.Service;

public interface IPermissionService
{
    Task<DataResult> GetAllAsync();
    Task<DataResult> CreatePermissionAsync(CreatePermissionRequest request);
    Task<DataResult> UpdatePermissionAsync(UpdatePermissionRequest request);
    Task<DataResult> DeletePermissionAsync(Guid id);
}