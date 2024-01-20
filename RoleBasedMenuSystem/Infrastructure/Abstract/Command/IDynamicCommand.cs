using RoleBasedMenuSystem.Core.Entities.Concrete;

namespace RoleBasedMenuSystem.Infrastructure.Abstract.Command;

public interface IDynamicCommand
{
    Task<(bool succeeded, Guid userId)> AddWithGuidIdentityAsync<T>(T entity) where T : BaseEntity<Guid>;
    Task<(bool succeeded, Guid userId)> AddAsync<T, TIdentityType>(T entity);
    Task<int> UpdateAsync<T>(T entity);
    Task<int> HardDeleteAsync<T>(int id);
}