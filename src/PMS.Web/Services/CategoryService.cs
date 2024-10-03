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

    public Task<Category> CreateCategory(Category category)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Category>> GetCategories()
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetCategory(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCategory(int id, Category category)
    {
        throw new NotImplementedException();
    }
}