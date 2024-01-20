using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoleBasedMenuSystem.Core.Entities.Concrete;
using RoleBasedMenuSystem.Model.Entities;

namespace RoleBasedMenuSystem.DataAccess.Concrete;

public class ProjectDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public ProjectDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
        optionsBuilder.UseSqlServer(_configuration.GetSection("Database:ConnectionString").Value);
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUserRole> AppUserRoles { get; set; }
    public DbSet<MenuEntity> AppMenus { get; set; }
    public DbSet<PermissionEntity> Permissions { get; set; }
    public DbSet<RolePermissionRelationEntity> RolePermissionRelations { get; set; }
    public DbSet<MenuRolePermissionRelationEntity> AppMenuPermissionRoleRelations { get; set; }
}

