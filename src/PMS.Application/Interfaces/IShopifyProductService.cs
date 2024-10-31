using PMS.Core.Entities;

namespace PMS.Application.Interfaces;

public interface IShopifyProductService
{
    Task<IReadOnlyList<Product>> GetAllShopifyProducts();
    Task<Product> GetShopifyProductById(string id);
    Task<Product> AddShopifyProduct(Product product);
    Task UpdateShopifyProduct(Product product);
    Task DeleteShopifyProduct(Product product);
    Task AddManyShopifyProducts(IEnumerable<Product> products);
    Task UpdateManyShopifyProducts(IEnumerable<Product> products);
    Task DeleteManyShopifyProducts(IEnumerable<Product> products);
}