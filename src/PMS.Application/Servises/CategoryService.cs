using PMS.Application.DTOs.Category;
using PMS.Application.Interfaces;
using PMS.Core.Repositories;

namespace PMS.Application.Servises
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public Task<CategoryDto> CreateCategory(CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryDto>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategory(int id, CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }
    }
}