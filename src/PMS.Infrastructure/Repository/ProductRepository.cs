using Microsoft.EntityFrameworkCore;
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
         public async Task<Product> GetBySkuAsync(string sku)
        {
            try
            {
                 var entity = await _dbContext.Set<Product>().FirstOrDefaultAsync(e => EF.Property<string>(e, "Sku") == sku);
                return entity!;
            }
            catch (Exception)
            {
                throw new InfrastructureException("Error loading entity");
            }
        }
    }
}