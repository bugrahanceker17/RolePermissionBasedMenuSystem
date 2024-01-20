namespace RoleBasedMenuSystem.Model.DataTransferObjects.MenuRolePermission;

public class SetRolePermissionToMenuRequest
{
    public Guid MenuId { get; set; }
    public Guid RolePermissionRelationId { get; set; }
}