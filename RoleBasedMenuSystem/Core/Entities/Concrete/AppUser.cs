using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoleBasedMenuSystem.Core.Entities.Abstract;

namespace RoleBasedMenuSystem.Core.Entities.Concrete;

[Table("AppUsers")]
public class AppUser : BaseEntity<Guid>, IEntity
{
    [StringLength(75)] public string FirstName { get; set; }
    [StringLength(75)] public string LastName { get; set; }
    [StringLength(20)] public string? PhoneNumber { get; set; }
    public bool? PhoneNumberConfirmed { get; set; }
    [StringLength(100)] public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    public string? UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public bool? LockoutEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
}