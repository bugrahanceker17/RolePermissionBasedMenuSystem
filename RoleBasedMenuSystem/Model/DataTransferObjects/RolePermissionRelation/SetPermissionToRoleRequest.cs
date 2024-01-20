namespace RoleBasedMenuSystem.Model.DataTransferObjects.RolePermissionRelation;

public class SetPermissionToRoleRequest
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}