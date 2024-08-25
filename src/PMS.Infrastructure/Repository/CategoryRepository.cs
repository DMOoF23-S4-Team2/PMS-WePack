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
    }
}