using System.ComponentModel.DataAnnotations;
using Dapper;
using RoleBasedMenuSystem.Core.Entities.Abstract;
using RoleBasedMenuSystem.Core.Entities.Concrete;

namespace RoleBasedMenuSystem.Model.Entities;

[Table("AppMenus")]
public class MenuEntity : BaseEntity<Guid>, IEntity
{
    [StringLength(100)] public string Name { get; set; }
    [StringLength(100)] public string Path { get; set; }
    [StringLength(100)] public string Icon { get; set; }
    public Guid? ParentId { get; set; }
}