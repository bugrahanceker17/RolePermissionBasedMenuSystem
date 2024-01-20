using Microsoft.AspNetCore.Identity;
using RoleBasedMenuSystem.Core.Entities.Concrete;
using RoleBasedMenuSystem.Core.Security.Hashing;
using RoleBasedMenuSystem.Core.Security.JWT;
using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Infrastructure.Abstract.Command;
using RoleBasedMenuSystem.Infrastructure.Abstract.Query;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Model.DataTransferObjects.Auth;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Service;

public class AuthService : IAuthService
{
    private readonly IDynamicCommand _dynamicCommand;
    private readonly IDynamicQuery _dynamicQuery;
    private readonly IUserQuery _userQuery;
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserCommand _userCommand;

    public AuthService(IDynamicQuery dynamicQuery, IUserQuery userQuery, ITokenHelper tokenHelper, IDynamicCommand dynamicCommand, IUserCommand userCommand)
    {
        _dynamicQuery = dynamicQuery;
        _userQuery = userQuery;
        _tokenHelper = tokenHelper;
        _dynamicCommand = dynamicCommand;
        _userCommand = userCommand;
    }

    public async Task<DataResult> RegisterAsync(RegisterRequest request)
    {
        DataResult dataResult = new DataResult();

        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
        
        AppUser entity = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = false,
            PhoneNumberConfirmed = false,
            Email = request.Email,
            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash
        };
        (bool succeeded, Guid userId) result = await _dynamicCommand.AddWithGuidIdentityAsync(entity);

        if (result.succeeded)
        {
            int addUserRoleCheck = await _userCommand.AddNewUserToStandardRoleAsync(result.userId);
            
            if (addUserRoleCheck > 0)
            {
                dataResult.Data = "Başarılı";
                return dataResult;
            }
            
            dataResult.ErrorMessageList.Add("Hata!!!!");
            return dataResult;
        }


        return dataResult;
    }

    public async Task<DataResult> LoginAsync(LoginRequest request)
    {
        DataResult dataResult = new();
        string fail = "İşlem başarısız";

        AppUser? user = await _userQuery.GetUserByUserNameAsync(request.Value);

        if (user is null)
        {
            dataResult.ErrorMessageList.Add(fail);
            return dataResult;
        }

        List<Guid>? userRole = await _userQuery.GetRoleIdByUserIdAsync(user.Id);

        if (userRole is null)
        {
            dataResult.ErrorMessageList.Add(fail);
            return dataResult;
        }

        if (!HashingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            dataResult.ErrorMessageList.Add("Hatalı parola!!!");
            return dataResult;
        }

        AccessToken token = _tokenHelper.CreateToken(user, userRole.ToList());

        if (string.IsNullOrEmpty(token.Token))
        {
            dataResult.ErrorMessageList.Add(fail);
            return dataResult;
        }

        dataResult.Data = token.Token;
        return dataResult;
    }

    public async Task<DataResult> AddRoleAsync(AddRoleRequest request)
    {
        DataResult dataResult = new DataResult();

        AppRole entity = new AppRole()
        {
            Name = request.Name
        };

        await _dynamicCommand.AddWithGuidIdentityAsync(entity);

        dataResult.Data = "Başarılı";
        return dataResult;
    }

    public async Task<DataResult> GetAllRoleAsync()
    {
        DataResult dataResult = new DataResult();

        List<AppRole> list = await _dynamicQuery.GetAllAsync<AppRole>("");

        if (list.Any())
        {
            dataResult.Data = list.Select(c => new
            {
                c.Id,
                c.Name
            });
            dataResult.Total = list.Count;
        }

        return dataResult;
    }

    public async Task<DataResult> AddRoleToUserAsync(AddRoleToUserRequest request)
    {
        DataResult dataResult = new DataResult();

        AppUserRole entity = new()
        {
            UserId = request.UserId,
            RoleId = request.RoleId

        };

        await _dynamicCommand.AddWithGuidIdentityAsync(entity);

        dataResult.Data = "Başarılı";
        return dataResult;
    }
}