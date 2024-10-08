using PMS.Web.Models;

namespace PMS.Web.Services.Interfaces;

public interface IProductService
{
    public Task<IEnumerable<Product>> GetProducts();
    public Task<Product> GetProduct(int id);
    public Task<Product> CreateProduct(Product product);
    public Task UpdateProduct(int id, Product product);
    public Task DeleteProduct(int id);
}