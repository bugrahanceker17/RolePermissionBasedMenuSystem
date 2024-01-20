using System.ComponentModel.DataAnnotations.Schema;
using RoleBasedMenuSystem.Core.Entities.Abstract;

namespace RoleBasedMenuSystem.Core.Entities.Concrete;

[Table("AppUserRoles")]
public class AppUserRole : BaseEntity<Guid>, IEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}