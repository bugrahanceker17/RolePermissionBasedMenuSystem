using RoleBasedMenuSystem.Model.Entities;

namespace RoleBasedMenuSystem;

public class PermissionSeed
{
    public List<PermissionEntity> PermissionList = new();

    public PermissionSeed()
    {
        PermissionList.Add(GenerateEntity(PermissionProperties.SatinAlmaMenusuGoruntuleme));
        PermissionList.Add(GenerateEntity(PermissionProperties.İstatistikleriGormeMenu));
        PermissionList.Add(GenerateEntity(PermissionProperties.ProjeMenusuGoruntuleme));
        PermissionList.Add(GenerateEntity(PermissionProperties.TumMenuleriGormeAdmin));
        PermissionList.Add(GenerateEntity(PermissionProperties.UrunEklemeYetkisi));
    }

    private static PermissionEntity GenerateEntity(PermissionProperties parameter)
    {
        return new PermissionEntity
        {
            CreatedAt = DateTime.Now,
            IsStatus = true,
            IsDeleted = false,
            Code = parameter.Code,
            Name = parameter.Name
        };
    }
}