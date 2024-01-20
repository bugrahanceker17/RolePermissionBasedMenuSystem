using AutoMapper;
using RoleBasedMenuSystem.Model.DataTransferObjects.Menu;
using RoleBasedMenuSystem.Model.DataTransferObjects.Permission;
using RoleBasedMenuSystem.Model.Entities;

namespace RoleBasedMenuSystem;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MenuEntity, UpdateMenuRequest>().ReverseMap();
        CreateMap<PermissionEntity, UpdatePermissionRequest>().ReverseMap();
    }
}