using PMS.Web.Models;
using PMS.Web.Services.Interfaces;

namespace PMS.Web.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Product> CreateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetProduct(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetProducts()
    {
        throw new NotImplementedException();
    }

    public Task UpdateProduct(int id, Product product)
    {
        throw new NotImplementedException();
    }
}
