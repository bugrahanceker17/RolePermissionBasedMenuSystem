using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Model.DataTransferObjects.Auth;

namespace RoleBasedMenuSystem.Infrastructure.Abstract.Service;

public interface IAuthService
{
    Task<DataResult> RegisterAsync(RegisterRequest request);
    Task<DataResult> LoginAsync(LoginRequest request);
    Task<DataResult> AddRoleAsync(AddRoleRequest request);
    Task<DataResult> GetAllRoleAsync();
    Task<DataResult> AddRoleToUserAsync(AddRoleToUserRequest request);
}