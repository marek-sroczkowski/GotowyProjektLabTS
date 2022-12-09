using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Persistence;
using LabAuthorizationTS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace LabAuthorizationTS.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext dbContext;

        public Repository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddAndSaveAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);

            return await SaveChangedAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null
                ? await dbContext.Set<TEntity>().AsNoTracking().ToListAsync()
                : await dbContext.Set<TEntity>().AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<TEntity> FindByIdAsync(long entityId)
        {
            var entity = await dbContext.Set<TEntity>().FindAsync(entityId);

            return entity;
        }

        public async Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null
                ? await dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync()
                : await dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<bool> RemoveAndSaveAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateAndSaveAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);

            return await SaveChangedAsync();
        }

        public async Task<bool> ContainsAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null
                ? await dbContext.Set<TEntity>().AnyAsync()
                : await dbContext.Set<TEntity>().AnyAsync(expression);
        }

        public async Task<bool> SaveChangedAsync()
        {
            try
            {
                var changesCount = await dbContext.SaveChangesAsync();

                return changesCount > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return false;
        }
    }
}