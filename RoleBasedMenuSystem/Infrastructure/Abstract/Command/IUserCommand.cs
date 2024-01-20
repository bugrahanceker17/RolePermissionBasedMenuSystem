namespace RoleBasedMenuSystem.Infrastructure.Abstract.Command;

public interface IUserCommand
{
    Task<int> AddNewUserToStandardRoleAsync(Guid id);
}