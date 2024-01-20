using System.ComponentModel.DataAnnotations.Schema;
using RoleBasedMenuSystem.Core.Entities.Abstract;

namespace RoleBasedMenuSystem.Core.Entities.Concrete;

[Table("AppRoles")]
public class AppRole : BaseEntity<Guid>, IEntity
{
    public string Name { get; set; }
}