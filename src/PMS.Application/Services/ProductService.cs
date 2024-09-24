using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Application.DTOs.Product;
using PMS.Application.Interfaces;
using PMS.Application.Mapper;
using PMS.Core.Entities;
using PMS.Core.Repositories;

namespace PMS.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<ProductDto> CreateProduct(ProductDto productDto)
        {
            await ValidateIfExist(productDto);

            var product = ObjectMapper.Mapper.Map<Product>(productDto);
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            //TODO - Implement Validation

            var newProduct = await _productRepository.AddAsync(product);

            return ObjectMapper.Mapper.Map<ProductDto>(newProduct);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var mappedProduct = ObjectMapper.Mapper.Map<ProductDto>(product);
            if (mappedProduct == null)
                throw new ArgumentNullException(nameof(mappedProduct));

            return mappedProduct;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            var mappedProducts = ObjectMapper.Mapper.Map<IEnumerable<ProductDto>>(products);
            if (mappedProducts == null)
                throw new ArgumentNullException(nameof(mappedProducts));

            return mappedProducts;
        }

        public async Task UpdateProduct(int id, ProductDto productDto)
        {
            var oldProduct = await _productRepository.GetByIdAsync(id);
            if (oldProduct == null)
                throw new ArgumentNullException(nameof(oldProduct));

            var newProduct = ObjectMapper.Mapper.Map<Product>(productDto);
            if (newProduct == null)
                throw new ArgumentNullException(nameof(newProduct));

            //TODO - Implement Validation

            await _productRepository.UpdateAsync(ObjectMapper.Mapper.Map(newProduct, oldProduct));
        }

        private async Task ValidateIfExist(ProductDto productDto)
        {
            if (productDto.Id != 0)
            {
                var product = await _productRepository.GetByIdAsync(productDto.Id);
                if (product != null)
                    throw new ArgumentNullException(nameof(product));
            }
        }
    }
}