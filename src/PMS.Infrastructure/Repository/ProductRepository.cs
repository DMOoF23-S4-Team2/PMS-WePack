using PMS.Core.Entities;
using PMS.Core.Repositories;
using PMS.Infrastructure.Data;
using PMS.Infrastructure.Repository.Base;

namespace PMS.Infrastructure.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(PMSContext dbContext) : base(dbContext)
        {
        }
    }
}