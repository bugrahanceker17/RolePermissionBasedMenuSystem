using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using RoleBasedMenuSystem.Core.Entities.Concrete;
using RoleBasedMenuSystem.Infrastructure.Abstract.Command;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Command;

public class UserCommand : IUserCommand
{
    private readonly string? _connectionString;

    public UserCommand(IConfiguration configuration)
    {
        _connectionString = configuration.GetSection("Database:ConnectionString").Value;
    }
    
    public async Task<int> AddNewUserToStandardRoleAsync(Guid id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        AppRole? _standardRole = await db.QueryFirstOrDefaultAsync<AppRole>("SELECT * FROM AppRoles WHERE Name = @Role", new { Role = "Standard" });

        if (_standardRole == null)
            return -1;

        AppUserRole entity = new AppUserRole
        {
            RoleId = _standardRole.Id,
            UserId = id
        };

        Guid result = await db.InsertAsync<Guid, AppUserRole>(entity);

        if (result != Guid.Empty)
            return 1;

        return -1;
    }
}