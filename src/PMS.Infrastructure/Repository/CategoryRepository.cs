using PMS.Core.Entities;
using PMS.Core.Repositories;
using PMS.Infrastructure.Data;
using PMS.Infrastructure.Repository.Base;

namespace PMS.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(PMSContext dbContext) : base(dbContext)
        {
        }

       public async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbContext.Set<Category>().FindAsync(id);
                return entity!;
            }
            catch (Exception)
            {
                throw new InfrastructureException("Error loading entity");
            }
        }
    }
}