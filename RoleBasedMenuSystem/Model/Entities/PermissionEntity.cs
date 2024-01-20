using System.ComponentModel.DataAnnotations.Schema;
using RoleBasedMenuSystem.Core.Entities.Abstract;
using RoleBasedMenuSystem.Core.Entities.Concrete;

namespace RoleBasedMenuSystem.Model.Entities;

[Table("Permissions")]
public class PermissionEntity: BaseEntity<Guid>, IEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
}