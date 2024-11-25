using System;
using PMS.Core.Entities;
using PMS.Core.Repositories.Base;

namespace PMS.Core.Repositories;

public interface IProductRepository : IRepository<Product>
{
    //REVIEW - Skal vi implementere en specifik metode til at uploade fra csv-fil?

    Task<Product> GetBySkuAsync(string sku);
    Task<IEnumerable<Product>> GetBySkusAsync(IEnumerable<string> skus);
    Task <IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<int> ids);
    Task AddManyAsync(IEnumerable<Product> entities);
    Task UpdateManyAsync(IEnumerable<Product> entities);
    Task DeleteManyAsync(IEnumerable<Product> entities);
}
