using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Infrastructure.Abstract.Command;
using RoleBasedMenuSystem.Infrastructure.Abstract.Query;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Model.DataTransferObjects.MenuRolePermission;
using RoleBasedMenuSystem.Model.DataTransferObjects.RolePermissionRelation;
using RoleBasedMenuSystem.Model.Entities;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Service;

public class RelationService : IRelationService
{
    private readonly IDynamicCommand _dynamicCommand;
    private readonly IDynamicQuery _dynamicQuery;

    public RelationService(IDynamicCommand dynamicCommand, IDynamicQuery dynamicQuery)
    {
        _dynamicCommand = dynamicCommand;
        _dynamicQuery = dynamicQuery;
    }

    public async Task<DataResult> SetRolePermissionToMenuAsync(SetRolePermissionToMenuRequest request)
    {
        DataResult dataResult = new DataResult();
        
        if (request.MenuId == null || request.MenuId == Guid.Empty || request.RolePermissionRelationId == null || request.RolePermissionRelationId == Guid.Empty)
        {
            dataResult.ErrorMessageList.Add("Hatalı parametre!!!");
            return dataResult;
        }

        MenuRolePermissionRelationEntity entity = new MenuRolePermissionRelationEntity()
        {
            RolePermissionRelationId = request.RolePermissionRelationId,
            MenuId = request.MenuId
        };

        (bool succeeded, Guid userId) result = await _dynamicCommand.AddWithGuidIdentityAsync(entity);
        
        if (result.succeeded)
        {
            dataResult.Data = "Başarılı";
            return dataResult;
        }

        dataResult.ErrorMessageList.Add("HATA!!!");
        return dataResult;
    }

    public async Task<DataResult> SetPermissionToRoleAsync(SetPermissionToRoleRequest request)
    {
        DataResult dataResult = new DataResult();

        RolePermissionRelationEntity entity = new()
        {
            PermissionId = request.PermissionId,
            RoleId = request.RoleId
        };

        (bool succeeded, Guid userId) res = await _dynamicCommand.AddWithGuidIdentityAsync(entity);

        if (res.succeeded)
        {
            dataResult.Data = "Başarılı";
            return dataResult;
        }
        
        dataResult.ErrorMessageList.Add("Hata!!!");
        return dataResult;
    }
}