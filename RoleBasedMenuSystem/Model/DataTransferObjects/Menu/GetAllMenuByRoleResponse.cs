namespace RoleBasedMenuSystem.Model.DataTransferObjects.Menu;

public class GetAllMenuByRoleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Icon { get; set; }
    public Guid? ParentId { get; set; }
}