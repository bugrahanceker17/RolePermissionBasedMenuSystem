namespace RoleBasedMenuSystem.Model.DataTransferObjects.Auth;

public class AddRoleToUserRequest
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}