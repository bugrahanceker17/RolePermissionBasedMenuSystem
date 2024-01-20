namespace RoleBasedMenuSystem.Model.DataTransferObjects.Menu;

public class CreateMenuRequest
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Icon { get; set; }
    public string? ParentId { get; set; }
}