
using Microsoft.EntityFrameworkCore;
using PMS.Core.Repositories.Base;
using PMS.Infrastructure.Data;

namespace PMS.Infrastructure.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly PMSContext _dbContext;

        public Repository(PMSContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception)
            {
                throw new InfrastructureException("Error loading entities");
            }
        }

        //NOTE - this method uses null forgiving operator (!) in the returned entity and should be handled in Application layer
        public async Task<T> GetBySkuAsync(string sku)
        {
            try
            {
                 var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(e => EF.Property<string>(e, "Sku") == sku);
                return entity!;
            }
            catch (Exception)
            {
                throw new InfrastructureException("Error loading entity");
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                throw new InfrastructureException("Error adding entity");
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InfrastructureException("Error updating entity");
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InfrastructureException("Error deleting entity");
            }
        }

        public async Task AddManyAsync(IEnumerable<T> entities)
        {
            try
            {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            throw new InfrastructureException("Error adding entities");
            }
        }

        public async Task UpdateManyAsync(IEnumerable<T> entities)
        {
            try
            {
            _dbContext.Set<T>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            throw new InfrastructureException("Error updating entities");
            }
        }

        public async Task DeleteManyAsync(IEnumerable<T> entities)
        {
            try
            {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            throw new InfrastructureException("Error deleting entities");
            }
        }
    }
}