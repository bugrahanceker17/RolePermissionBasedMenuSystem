using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using RoleBasedMenuSystem.Core.Entities.Concrete;
using RoleBasedMenuSystem.Core.Extensions;
using RoleBasedMenuSystem.Infrastructure.Abstract.Command;

namespace RoleBasedMenuSystem.Infrastructure.Concrete.Command;

public class DynamicCommand : IDynamicCommand
{
    private readonly string? _connectionString;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DynamicCommand(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _connectionString = configuration.GetSection("Database:ConnectionString").Value;
    }

    public async Task<(bool succeeded, Guid userId)> AddWithGuidIdentityAsync<T>(T entity) where T : BaseEntity<Guid>
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var userId = _httpContextAccessor.AccessToken().userId;
        
        entity.CreatedBy = string.IsNullOrEmpty(userId) ? null : userId;
        entity.CreatedAt = DateTime.Now;
        entity.IsDeleted = false;
        entity.IsStatus = true;
        
        Guid result = await db.InsertAsync<Guid, T>(entity);

        if (result != Guid.Empty)
            return (true, new Guid(result.ToString()));

        return (false, Guid.Empty);
    }
    public async Task<(bool succeeded, Guid userId)> AddAsync<T, TIdentityType>(T entity)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        TIdentityType result = await db.InsertAsync<TIdentityType, T>(entity);

        if (result != null)
            return (true, new Guid(result.ToString()));

        return (false, Guid.Empty);
    }

    public async Task<int> UpdateAsync<T>(T entity)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        int? result = await db.UpdateAsync(entity);
        return result.GetValueOrDefault();
    }

    public async Task<int> HardDeleteAsync<T>(int id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        int? result = await db.DeleteAsync<T>(id);
        return result.GetValueOrDefault();
    }
}