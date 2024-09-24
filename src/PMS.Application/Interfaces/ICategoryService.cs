using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Application.DTOs.Category;

namespace PMS.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDto>> GetCategories();
        public Task<CategoryDto> GetCategory(int id);
        public Task<CategoryDto> CreateCategory(CategoryDto categoryDto);
        public Task UpdateCategory(int id, CategoryDto categoryDto);
        public Task DeleteCategory(int id);
    }
}