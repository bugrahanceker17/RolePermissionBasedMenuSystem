namespace RoleBasedMenuSystem.Model.DataTransferObjects.Permission;

public class UpdatePermissionRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}