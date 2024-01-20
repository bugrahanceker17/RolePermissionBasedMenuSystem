using AutoMapper;
using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Infrastructure.Abstract.Command;
using RoleBasedMenuSystem.Infrastructure.Abstract.Query;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Model.DataTransferObjects.Permission;
using RoleBasedMenuSystem.Model.Entities;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Service;

public class PermissionService : IPermissionService
{
    private readonly IDynamicCommand _dynamicCommand;
    private readonly IDynamicQuery _dynamicQuery;
    private readonly IMapper _mapper;
    private readonly IUserQuery _userQuery;

    public PermissionService(IDynamicCommand dynamicCommand, IDynamicQuery dynamicQuery, IMapper mapper, IUserQuery userQuery)
    {
        _dynamicCommand = dynamicCommand;
        _dynamicQuery = dynamicQuery;
        _mapper = mapper;
        _userQuery = userQuery;
    }

    public async Task<DataResult> GetAllAsync()
    {
        DataResult dataResult = new DataResult();

        List<PermissionEntity> list = await _dynamicQuery.GetAllAsync<PermissionEntity>("");

        if (list.Any())
        {
            dataResult.Data = list.Select(c => new
            {
                c.Id,
                c.Name
            }).ToList();
            dataResult.Total = list.Count;
        }

        return dataResult;
    }

    public async Task<DataResult> CreatePermissionAsync(CreatePermissionRequest request)
    {
        DataResult dataResult = new DataResult();

        PermissionEntity entity = new PermissionEntity
        {
            Code = request.Code,
            Name = request.Name,
        };

        (bool succeeded, Guid userId) result = await _dynamicCommand.AddWithGuidIdentityAsync(entity);

        if (result.succeeded)
        {
            List<(Guid id, string name)> roleList = await _userQuery.GetAllRoles();

            RolePermissionRelationEntity rolePermissionRelationEntity = new RolePermissionRelationEntity()
            {
                PermissionId = result.userId,
                RoleId = roleList.First(c => c.name.Equals("Admin")).id
            };

            await _dynamicCommand.AddWithGuidIdentityAsync(rolePermissionRelationEntity);

            dataResult.Data = "Başarılı";
            return dataResult;
        }

        dataResult.ErrorMessageList.Add("HATA!!!");
        return dataResult;
    }

    public async Task<DataResult> UpdatePermissionAsync(UpdatePermissionRequest request)
    {
        DataResult dataResult = new DataResult();

        PermissionEntity data = await _dynamicQuery.GetAsync<PermissionEntity>(request.Id);

        if (data is null)
        {
            dataResult.ErrorMessageList.Add("BULUNAMADI!!!");
            return dataResult;
        }

        _mapper.Map(request, data);

        int result = await _dynamicCommand.UpdateAsync(data);

        if (result > 0)
        {
            dataResult.Data = "Başarılı";
            return dataResult;
        }

        dataResult.ErrorMessageList.Add("HATA!!!");
        return dataResult;
    }

    public async Task<DataResult> DeletePermissionAsync(Guid id)
    {
        DataResult dataResult = new DataResult();

        PermissionEntity data = await _dynamicQuery.GetAsync<PermissionEntity>(id);
        
        if (data is null)
        {
            dataResult.ErrorMessageList.Add("BULUNAMADI!!!");
            return dataResult;
        }

        data.IsDeleted = true;
        
        int result = await _dynamicCommand.UpdateAsync(data);

        if (result > 0)
        {
            dataResult.Data = "Başarılı";
            return dataResult;
        }

        dataResult.ErrorMessageList.Add("HATA!!!");
        return dataResult;
    }
}