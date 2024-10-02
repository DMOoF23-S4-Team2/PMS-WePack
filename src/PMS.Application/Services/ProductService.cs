using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using PMS.Application.DTOs.Product;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Application.Mapper;
using PMS.Application.Validators;
using PMS.Core.Entities;
using PMS.Core.Repositories;

namespace PMS.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductValidator _productValidator;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _productValidator = new ProductValidator();
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            await ValidateIfExist(productDto);
            var product = MappedEntityOf(productDto);
            await ValidateEntity(product);
            var newProduct = await CreateEntityInRepository(product);
            return MappedDtoOf(newProduct);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await GetEntityFromRepositoryWith(id);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await GetEntityFromRepositoryWith(id);
            return MappedDtoOf(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await GetAllEntitiesFromRepository();
            return MappedDtoOf(products);
        }

        public async Task UpdateProduct(int id, ProductDto productDto)
        {
            var oldProduct = await GetEntityFromRepositoryWith(id);
            var newProduct = MappedEntityOf(productDto);
            await ValidateEntity(newProduct);
            await UpdateEntityInRepository(newProduct, oldProduct);
        }

        //!SECTION Private Methods
        private async Task ValidateIfExist(ProductDto productDto)
        {
            ThrowArgument.ExceptionIfZero(productDto.Id);
            var product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product != null)
                throw new ValidationException("Product already exists.");
        }

        private static IEnumerable<ProductDto> MappedDtoOf(IEnumerable<Product> products)
        {
            var productDtos = ObjectMapper.Mapper.Map<IEnumerable<ProductDto>>(products);
            ThrowArgument.NullExceptionIfNull(productDtos);
            return productDtos;
        }

        private static ProductDto MappedDtoOf(Product product)
        {
            var productDto = ObjectMapper.Mapper.Map<ProductDto>(product);
            ThrowArgument.NullExceptionIfNull(productDto);
            return productDto;
        }

        private static Product MappedEntityOf(ProductDto productDto)
        {
            var product = ObjectMapper.Mapper.Map<Product>(productDto);
            ThrowArgument.NullExceptionIfNull(product);
            return product;
        }

        private async Task ValidateEntity(Product product)
        {
            var validationResult = await _productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }

        private async Task<Product> CreateEntityInRepository(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        private async Task<Product> GetEntityFromRepositoryWith(int id)
        {
            ThrowArgument.ExceptionIfZero(id);
            var product = await _productRepository.GetByIdAsync(id);
            ThrowArgument.NullExceptionIfNull(product);
            return product;
        }

        private async Task<IEnumerable<Product>> GetAllEntitiesFromRepository()
        {
            var products = await _productRepository.GetAllAsync();
            ThrowArgument.NullExceptionIfNull(products);
            return products;
        }

        private async Task UpdateEntityInRepository(Product newProduct, Product oldProduct)
        {
            var mappedProduct = ObjectMapper.Mapper.Map(newProduct, oldProduct);
            await _productRepository.UpdateAsync(mappedProduct);
        }
    }
}