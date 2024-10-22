using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Core.Entities;

namespace PMS.Application.Interfaces
{
    public interface IShopifyProductService
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task PatchProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task AddManyProductsAsync(IEnumerable<Product> product);
        Task UpdateManyProductsAsync(IEnumerable<Product> product);
        Task DeleteManyProductsAsync(IEnumerable<Product> product);
    }
}