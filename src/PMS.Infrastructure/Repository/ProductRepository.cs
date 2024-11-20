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
        public async Task AddManyAsync(IEnumerable<Product> products)
        {
            try
            {
            await _dbContext.Set<Product>().AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            throw new InfrastructureException("Error adding entities");
            }
        }

        public async Task UpdateManyAsync(IEnumerable<Product> products)
        {
            try
            {
            _dbContext.Set<Product>().UpdateRange(products);
            await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            throw new InfrastructureException("Error updating entities");
            }
        }

        public async Task DeleteManyAsync(IEnumerable<Product> products)
        {
            try
            {
            // Extract SKUs from the provided products
            var skus = products.Select(p => p.Sku).ToList();

            // Fetch entities matching the provided SKUs
            var entitiesToDelete = await _dbContext.Set<Product>()
                .Where(p => skus.Contains(p.Sku))
                .ToListAsync();

            if (entitiesToDelete.Any())
            {
                _dbContext.Set<Product>().RemoveRange(entitiesToDelete);
                await _dbContext.SaveChangesAsync();
            }
            }
            catch (Exception)
            {
            throw new InfrastructureException("Error deleting entities");
            }
        }
    }
    
}