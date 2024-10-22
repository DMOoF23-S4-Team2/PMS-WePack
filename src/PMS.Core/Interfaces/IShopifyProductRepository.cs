using PMS.Core.Entities;

namespace PMS.Core.Interfaces;

public interface IShopifyProductRepository
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