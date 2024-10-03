using PMS.Web.Models;

namespace PMS.Web.Services.Interfaces;

public interface ICategoryService
{
    public Task<IEnumerable<Category>> GetCategories();
    public Task<Category> GetCategory(int id);
    public Task<Category> CreateCategory(Category category);
    public Task UpdateCategory(int id, Category category);
    public Task DeleteCategory(int id);
}