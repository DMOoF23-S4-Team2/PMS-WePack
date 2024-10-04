using PMS.Web.Models;
using PMS.Web.Services.Interfaces;

namespace PMS.Web.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Category> CreateCategory(Category category)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/categories", category);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Category>()
                ?? throw new Exception("Error creating category");
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating category", ex);
        }
    }

    public async Task DeleteCategory(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting category", ex);
        }
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/categories");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Category>>()
                ?? throw new Exception("Error getting categories");
        }
        catch (Exception ex)
        {
            throw new Exception("Error getting categories", ex);
        }
    }

    public async Task<Category> GetCategory(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Category>()
                ?? throw new Exception("Error getting category");
        }
        catch (Exception ex)
        {
            throw new Exception("Error getting category", ex);
        }
    }

    public async Task UpdateCategory(int id, Category category)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/categories/{id}", category);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating category", ex);
        }
    }
}