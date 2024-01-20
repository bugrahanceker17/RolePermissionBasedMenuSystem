namespace RoleBasedMenuSystem.Core.Utilities.Attribute;


public class CustomPermissionControlAttribute : System.Attribute
{
    public string[]? Permissions { get; set; }
    public bool MustLogin { get; set; }

    public CustomPermissionControlAttribute(bool mustLogin, params string[] permissions)
    {
        Permissions = permissions;
        MustLogin = mustLogin;
    }

    public CustomPermissionControlAttribute(bool mustLogin)
    {
        MustLogin = mustLogin;
    }
}