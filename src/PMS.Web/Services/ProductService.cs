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

    public async Task<Product> CreateProduct(Product product)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", product);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>()
                ?? throw new Exception("Error creating product");
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating product", ex);
        }
    }

    public async Task DeleteProduct(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting product", ex);
        }
    }

    public async Task<Product> GetProduct(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>()
                ?? throw new Exception("Error getting product");
        }
        catch (Exception ex)
        {
            throw new Exception("Error getting product", ex);
        }
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/products");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>()
                ?? throw new Exception("Error getting products");
        }
        catch (Exception ex)
        {
            throw new Exception("Error getting products", ex);
        }
    }

    public async Task UpdateProduct(int id, Product product)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating product", ex);
        }
    }
}
