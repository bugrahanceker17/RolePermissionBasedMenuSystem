using System.ComponentModel.DataAnnotations.Schema;
using RoleBasedMenuSystem.Core.Entities.Abstract;
using RoleBasedMenuSystem.Core.Entities.Concrete;

namespace RoleBasedMenuSystem.Model.Entities;

[Table("AppMenuPermissionRoleRelations")]
public class MenuRolePermissionRelationEntity: BaseEntity<Guid>, IEntity
{
    public Guid MenuId { get; set; }
    public Guid RolePermissionRelationId { get; set; }
}