using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Model.DataTransferObjects.MenuRolePermission;
using RoleBasedMenuSystem.Model.DataTransferObjects.RolePermissionRelation;

namespace RoleBasedMenuSystem.Infrastructure.Abstract.Service;

public interface IRelationService
{
    Task<DataResult> SetRolePermissionToMenuAsync(SetRolePermissionToMenuRequest request);
    Task<DataResult> SetPermissionToRoleAsync(SetPermissionToRoleRequest request);
}