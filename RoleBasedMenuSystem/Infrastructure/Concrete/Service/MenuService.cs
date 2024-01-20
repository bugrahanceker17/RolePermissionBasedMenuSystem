using AutoMapper;
using RoleBasedMenuSystem.Core.Entities.Concrete;
using RoleBasedMenuSystem.Core.Extensions;
using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Infrastructure.Abstract.Command;
using RoleBasedMenuSystem.Infrastructure.Abstract.Query;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Model.DataTransferObjects.Menu;
using RoleBasedMenuSystem.Model.Entities;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Service;

public class MenuService : IMenuService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDynamicCommand _dynamicCommand;
    private readonly IDynamicQuery _dynamicQuery;
    private readonly IMapper _mapper;

    public MenuService(IDynamicCommand dynamicCommand, IDynamicQuery dynamicQuery, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dynamicCommand = dynamicCommand;
        _dynamicQuery = dynamicQuery;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DataResult> GetAllAsync()
    {
        DataResult dataResult = new DataResult();

        List<MenuEntity> list = await _dynamicQuery.GetAllAsync<MenuEntity>("");

        if (list.Any())
        {
            dataResult.Data = list.Select(c => new
            {
                c.Id,
                c.Icon,
                c.Name,
                c.Path,
                c.ParentId
            }).ToList();
            dataResult.Total = list.Count;
        }

        return dataResult;
    }

    public async Task<DataResult> CreateMenuAsync(CreateMenuRequest request)
    {
        DataResult dataResult = new DataResult();

        MenuEntity entity = new MenuEntity()
        {
            Icon = request.Icon,
            ParentId = !string.IsNullOrEmpty(request.ParentId) ? new Guid(request.ParentId) : null,
            Name = request.Name,
            Path = request.Path,
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

    public async Task<DataResult> UpdateMenuAsync(UpdateMenuRequest request)
    {
        DataResult dataResult = new DataResult();

        MenuEntity data = await _dynamicQuery.GetAsync<MenuEntity>(request.Id);

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

    public async Task<DataResult> DeleteMenuAsync(Guid id)
    {
        DataResult dataResult = new DataResult();

        MenuEntity data = await _dynamicQuery.GetAsync<MenuEntity>(id);
        
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

    public async Task<DataResult> GetAllMenuByPermissionAsync()
    {
        DataResult dataResult = new DataResult();

        string userId = _httpContextAccessor.AccessToken().userId;

        List<AppUserRole> userRole = await _dynamicQuery.GetAllAsync<AppUserRole>("IsStatus = 1 AND IsDeleted = 0 AND UserId = @UserId", new { UserId = new Guid(userId) });
        
        if (userRole == null || !userRole.Any())
            return dataResult;
        
        List<Guid> roleIdList = userRole.Select(c => c.RoleId).ToList();
        List<RolePermissionRelationEntity> rolePermissionRelationList = await _dynamicQuery.GetAllAsync<RolePermissionRelationEntity>
            ("IsStatus = 1 AND IsDeleted = 0 AND RoleId IN @RoleIds", new { RoleIds = roleIdList });
        
        if(rolePermissionRelationList == null || !rolePermissionRelationList.Any())
            return dataResult;

        List<Guid> rolePermissionRelationIdList = rolePermissionRelationList.Select(c => c.Id).ToList();
        List<MenuRolePermissionRelationEntity> appMenuRolePermissionRelation = await _dynamicQuery.GetAllAsync<MenuRolePermissionRelationEntity>
            ("IsStatus = 1 AND IsDeleted = 0 AND RolePermissionRelationId IN @Ids", new {Ids = rolePermissionRelationIdList });
        
        if(appMenuRolePermissionRelation == null || !appMenuRolePermissionRelation.Any())
            return dataResult;

        List<Guid> menuIdList = appMenuRolePermissionRelation.Select(c => c.MenuId).ToList();
        var menuList = await _dynamicQuery.GetAllAsync<MenuEntity>
            ("IsStatus = 1 AND IsDeleted = 0 AND Id IN @MenuIds", new { MenuIds = menuIdList });

        if (menuList != null && menuList.Any())
        {
            dataResult.Data = menuList.Select(c => new GetAllMenuByRoleResponse
            {
                Id = c.Id,
                Icon = c.Icon,
                Name = c.Name,
                Path = c.Path,
                ParentId = c.ParentId
            });
        }
        
        return dataResult;
    }
}