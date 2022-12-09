using LabAuthorizationTS.Models.Entities;
using System.Linq.Expressions;

namespace LabAuthorizationTS.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<bool> AddAndSaveAsync(TEntity entity);
        Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> expression = null);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> expression = null);
        Task<TEntity> FindByIdAsync(long entityId);
        Task<bool> RemoveAndSaveAsync(TEntity entity);
        Task<bool> UpdateAndSaveAsync(TEntity entity);
        Task<bool> ContainsAsync(Expression<Func<TEntity, bool>> expression = null);
    }
}