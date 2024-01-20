using System.Data;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using RoleBasedMenuSystem.Core.Entities.Concrete;
using RoleBasedMenuSystem.Infrastructure.Abstract.Query;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Query;

public class UserQuery : IUserQuery
{
    private readonly string _connectionString;

    public UserQuery(IConfiguration configuration)
    {
        _connectionString = configuration.GetSection("Database:ConnectionString").Value;
    }

    
    public async Task<List<Guid>> GetRoleIdByUserIdAsync(Guid id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        StringBuilder sql = new StringBuilder();

        sql.Append("SELECT R.Id AS RoleId FROM AppUsers U ");
        sql.Append("INNER JOIN AppUserRoles UR ON UR.UserId = U.Id ");
        sql.Append("INNER JOIN AppRoles R ON R.Id = UR.RoleId ");
        sql.Append("WHERE U.IsStatus = 1 AND U.IsDeleted = 0 AND U.Id = @CustomerId ");

        return (await db.QueryAsync<Guid>(sql.ToString(), new { CustomerId = id })).ToList();
    }

    public async Task<List<(Guid id, string name)>> GetAllRoles()
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        return (await db.QueryAsync<(Guid, string)>("SELECT Id, Name FROM AppRoles")).ToList();
    }

    public async Task<AppUser> GetUserByUserNameAsync(string userName)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        return await db.QueryFirstOrDefaultAsync<AppUser>("SELECT * FROM AppUsers WHERE UserName = @Value", new { Value = userName });
    }
}