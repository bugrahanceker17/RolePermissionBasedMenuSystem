using System.ComponentModel.DataAnnotations.Schema;
using RoleBasedMenuSystem.Core.Entities.Abstract;
using RoleBasedMenuSystem.Core.Entities.Concrete;

namespace RoleBasedMenuSystem.Model.Entities;

[Table("RolePermissionRelations")]
public class RolePermissionRelationEntity: BaseEntity<Guid>, IEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}