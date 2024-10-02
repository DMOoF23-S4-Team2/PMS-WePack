using AutoMapper;
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
            _categoryValidator = new CategoryValidator();
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto categoryDto)
        {
            await ValidateIfExist(categoryDto);
            var category = MappedEntityOf(categoryDto);
            await ValidateEntity(category);
            var newCategory = await AddEntityToRepository(category);
            return MappedDtoOf(newCategory);
        }

        public async Task DeleteCategory(int id)
        {
            var category = await GetEntityFromRepositoryWith(id);
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = await GetAllEntityFromRepository();
            return MappedDtoOf(categories);
        }

        public async Task<CategoryDto> GetCategory(int id)
        {
            var category = await GetEntityFromRepositoryWith(id);
            return MappedDtoOf(category);
        }

        public async Task UpdateCategory(int id, CategoryDto categoryDto)
        {
            var oldCategory = await GetEntityFromRepositoryWith(id);
            var newCategory = MappedEntityOf(categoryDto);
            await ValidateEntity(newCategory);
            await UpdateEntityInRepository(newCategory, oldCategory);
        }

        //!SECTION: Private methods
        private async Task ValidateIfExist(CategoryDto categoryDto)
        {
            if (categoryDto.Id != 0)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
                if (category != null)
                    throw new ArgumentException("Category already exists");
            }
        }

        private static Category MappedEntityOf(CategoryDto categoryDto)
        {
            var category = ObjectMapper.Mapper.Map<Category>(categoryDto);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            return category;
        }

        private static IEnumerable<CategoryDto> MappedDtoOf(IEnumerable<Category> category)
        {
            var categoryDtos = ObjectMapper.Mapper.Map<IEnumerable<CategoryDto>>(category);
            if (categoryDtos == null)
            {
                throw new ArgumentNullException(nameof(categoryDtos));
            }
            return categoryDtos;
        }

        private static CategoryDto MappedDtoOf(Category category)
        {
            var categoryDto = ObjectMapper.Mapper.Map<CategoryDto>(category);
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto));
            }
            return categoryDto;
        }

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
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            return category;
        }

        private async Task<IEnumerable<Category>> GetAllEntityFromRepository()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null)
                throw new ArgumentNullException(nameof(categories));

            return categories;
        }

        private async Task UpdateEntityInRepository(Category newCategory, Category oldCategory)
        {
            var mappedCategory = ObjectMapper.Mapper.Map(newCategory, oldCategory);
            await _categoryRepository.UpdateAsync(mappedCategory);
        }
    }
}