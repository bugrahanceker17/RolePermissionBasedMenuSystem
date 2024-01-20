using System.Data;
using Microsoft.Data.SqlClient;
using RoleBasedMenuSystem.Infrastructure.Abstract.Query;
using RoleBasedMenuSystem.Model.DataTransferObjects.Menu;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Query;

public class MenuQuery : IMenuQuery
{
    private readonly string _connectionString;

    public MenuQuery(IConfiguration configuration)
    {
        _connectionString = configuration.GetSection("Database:ConnectionString").Value;
        
    }
    public async Task<List<GetAllMenuByRoleResponse>> GetAllByRoleAsync(Guid userId)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        return new List<GetAllMenuByRoleResponse>();
    }
}