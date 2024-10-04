using AutoMapper;
using FluentValidation;
using PMS.Application.DTOs.Category;
using PMS.Application.Exceptions;
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
            _categoryValidator = new CategoryValidator();
        }

        public async Task<CategoryWithoutIdDto> CreateCategory(CategoryWithoutIdDto categoryDto)
        {
            var category = ObjectMapper.Mapper.Map<Category>(categoryDto);
            await ValidateEntity(category);
            var newCategory = await AddEntityToRepository(category);
            var newCategoryDto = ObjectMapper.Mapper.Map<CategoryWithoutIdDto>(newCategory);
            ThrowArgument.ExceptionIfNull(newCategoryDto);
            return newCategoryDto;
        }

        public async Task DeleteCategory(int id)
        {
            var category = await GetEntityFromRepositoryWith(id);
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = await GetAllEntityFromRepository();
            var categoriesDto = ObjectMapper.Mapper.Map<IEnumerable<CategoryDto>>(categories);
            ThrowArgument.ExceptionIfNull(categoriesDto);
            return categoriesDto;
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            var category = await GetEntityFromRepositoryWith(id);
            var categoryDto = ObjectMapper.Mapper.Map<CategoryDto>(category);
            ThrowArgument.ExceptionIfNull(categoryDto);
            return categoryDto;
        }

        public async Task UpdateCategory(int id, CategoryWithoutIdDto categoryDto)
        {
            var oldCategory = await GetEntityFromRepositoryWith(id);
            var newCategory = ObjectMapper.Mapper.Map<Category>(categoryDto);
            await ValidateEntity(newCategory);
            await UpdateEntityInRepository(newCategory, oldCategory);
        }

        //!SECTION: Private methods
        private async Task ValidateEntity(Category category)
        {
            var validationResult = await _categoryValidator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        private async Task<Category> AddEntityToRepository(Category category)
        {
            return await _categoryRepository.AddAsync(category);
        }

        private async Task<Category> GetEntityFromRepositoryWith(int id)
        {
            ThrowArgument.ExceptionIfZero(id);
            var category = await _categoryRepository.GetByIdAsync(id);
            ThrowArgument.ExceptionIfNull(category);
            return category;
        }

        private async Task<IEnumerable<Category>> GetAllEntityFromRepository()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ThrowArgument.ExceptionIfNull(categories);
            return categories;
        }

        private async Task UpdateEntityInRepository(Category newCategory, Category oldCategory)
        {
            var mappedCategory = ObjectMapper.Mapper.Map(newCategory, oldCategory);
            await _categoryRepository.UpdateAsync(mappedCategory);
        }
    }
}