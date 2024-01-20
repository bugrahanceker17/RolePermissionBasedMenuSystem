using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using RoleBasedMenuSystem.Infrastructure.Abstract.Query;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Query;

public class DynamicQuery : IDynamicQuery
{
    private readonly string _connectionString;

    public DynamicQuery(IConfiguration configuration)
    {
        _connectionString = configuration.GetSection("Database:ConnectionString").Value;
    }
        
    public async Task<T> GetAsync<T>(object id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        return await db.GetAsync<T>(id);
    }

    public async Task<List<T>> GetAllAsync<T>(string whereCondition, object? parameter = null)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
            
        string def = "WHERE IsStatus = 1 AND IsDeleted = 0";

        string baseWhereCondition = string.IsNullOrEmpty(whereCondition)
            ? def
            : whereCondition.Contains("WHERE ")
                ? whereCondition
                : $"WHERE {whereCondition} ";
            
        return (await db.GetListAsync<T>(baseWhereCondition, parameter)).ToList();
    }

    public async Task<(List<T> record, int count)> GetAllByPaginationAsync<T>(int page, int pageSize, string whereCondition, string orderBy)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
            
        string def = "WHERE IsStatus = 1 AND IsDeleted = 0";

        string baseWhereCondition = string.IsNullOrEmpty(whereCondition)
            ? def
            : whereCondition.Contains("WHERE ")
                ? whereCondition
                : $"WHERE {whereCondition} ";
            
        int count = await db.RecordCountAsync<T>(baseWhereCondition);

        List<T> record = (await db.GetListPagedAsync<T>(page, pageSize, baseWhereCondition, orderBy)).ToList();

        return (record, count);
    }
}