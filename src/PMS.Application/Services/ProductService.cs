using System.Net.WebSockets;
using AutoMapper.Execution;
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

        public async Task<ProductWithoutIdDto> CreateProduct(ProductWithoutIdDto productDto)
        {
            var product = MappedEntityOf(productDto);
            await ValidateEntity(product);
            var newProduct = await CreateEntityInRepository(product);
            var newProductDto = ObjectMapper.Mapper.Map<ProductWithoutIdDto>(newProduct);
            ThrowArgument.ExceptionIfNull(newProductDto);
            return newProductDto;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await GetEntityFromRepositoryWith(id);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await GetEntityFromRepositoryWith(id);
            var productDto = ObjectMapper.Mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await GetAllEntitiesFromRepository();
            var productsDto = ObjectMapper.Mapper.Map<IEnumerable<ProductDto>>(products);
            ThrowArgument.ExceptionIfNull(productsDto);
            return productsDto;
        }

        public async Task UpdateProduct(int id, ProductWithoutIdDto productDto)
        {
            var oldProduct = await GetEntityFromRepositoryWith(id);
            var newProduct = MappedEntityOf(productDto);
            await ValidateEntity(newProduct);
            await UpdateEntityInRepository(productDto, oldProduct);
        }

        public async Task AddManyProducts(IEnumerable<ProductWithoutIdDto> productDtos)
        {
            var products = productDtos.Select(MappedEntityOf).ToList();
            await ValidateEntity(products);
            await _productRepository.AddManyAsync(products);
        }

        public async Task UpdateManyProducts(IEnumerable<ProductDto> productDtos)
        {
            var products = ObjectMapper.Mapper.Map<IEnumerable<Product>>(productDtos);
            await ValidateEntity(products);
            await _productRepository.UpdateManyAsync(products);
        }

        public async Task DeleteManyProducts(IEnumerable<ProductDto> productDtos)
        {
            var products = ObjectMapper.Mapper.Map<IEnumerable<Product>>(productDtos);
            await _productRepository.DeleteManyAsync(products);
        }

        //!SECTION Private Methods
        private static Product MappedEntityOf(object productDto)
        {
            if (productDto is ProductWithoutIdDto or Product)
            {
                var product = ObjectMapper.Mapper.Map<Product>(productDto);
                ThrowArgument.ExceptionIfNull(product);
                return product;
            }

            throw new ArgumentException($"Invalid type");
        }

        private async Task ValidateEntity(Product product)
        {
            var validationResult = await _productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
        private async Task ValidateEntity(IEnumerable<Product> products)
        {
            var validationTasks = products.Select(async product =>
            {
                var validationResult = await _productValidator.ValidateAsync(product);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            });

            await Task.WhenAll(validationTasks);
        }

        private async Task<Product> CreateEntityInRepository(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        private async Task<Product> GetEntityFromRepositoryWith(int id)
        {
            ThrowArgument.ExceptionIfZero(id);
            var product = await _productRepository.GetByIdAsync(id);
            ThrowArgument.ExceptionIfNull(product);
            return product;
        }

        private async Task<IEnumerable<Product>> GetAllEntitiesFromRepository()
        {
            var products = await _productRepository.GetAllAsync();
            ThrowArgument.ExceptionIfNull(products);
            return products;
        }

        private async Task UpdateEntityInRepository(ProductWithoutIdDto productDto, Product oldProduct)
        {
            var mappedProduct = ObjectMapper.Mapper.Map(productDto, oldProduct);
            await _productRepository.UpdateAsync(mappedProduct);
        }
    }
}