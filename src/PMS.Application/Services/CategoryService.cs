using FluentValidation;
using PMS.Application.DTOs.Category;
using PMS.Application.Interfaces;
using PMS.Application.Mapper;
using PMS.Application.Validators;
using PMS.Core.Entities;
using PMS.Core.Repositories;

namespace PMS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryValidator _categoryValidator;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto categoryDto)
        {
            await ValidateIfExist(categoryDto);

            var category = ObjectMapper.Mapper.Map<Category>(categoryDto);
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var validationResult = await _categoryValidator.ValidateAsync(category);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var newCategory = await _categoryRepository.AddAsync(category);

            return ObjectMapper.Mapper.Map<CategoryDto>(newCategory);
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null)
                throw new ArgumentNullException(nameof(categories));

            var mappedCategories = ObjectMapper.Mapper.Map<IEnumerable<CategoryDto>>(categories);
            if (mappedCategories == null)
                throw new ArgumentNullException(nameof(mappedCategories));

            return mappedCategories;
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var mappedCategory = ObjectMapper.Mapper.Map<CategoryDto>(category);
            if (mappedCategory == null)
                throw new ArgumentNullException(nameof(mappedCategory));

            return mappedCategory;
        }

        public async Task UpdateCategory(int id, CategoryDto categoryDto)
        {
            var oldCategory = await _categoryRepository.GetByIdAsync(id);
            if (oldCategory == null)
                throw new ArgumentNullException(nameof(oldCategory));

            var newCategory = ObjectMapper.Mapper.Map<Category>(categoryDto);
            if (newCategory == null)
                throw new ArgumentNullException(nameof(newCategory));

            var validationResult = await _categoryValidator.ValidateAsync(newCategory);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _categoryRepository.UpdateAsync(ObjectMapper.Mapper.Map(newCategory, oldCategory));

        }

        private async Task ValidateIfExist(CategoryDto categoryDto)
        {
            if (categoryDto.Id != 0)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
                if (category != null)
                    throw new ArgumentNullException(nameof(category));
            }
        }
    }
}