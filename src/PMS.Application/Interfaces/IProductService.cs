using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();
        public Task<ProductDto> GetProduct(string sku);
        public Task<ProductWithoutIdDto> CreateProduct(ProductWithoutIdDto productDto);
        public Task UpdateProduct(string sku, ProductWithoutIdDto productDto);
        public Task DeleteProduct(string sku);
        public Task AddManyProducts(IEnumerable<ProductWithoutIdDto> productDtos);
        public Task UpdateManyProducts(IEnumerable<ProductWithoutIdDto> productDtos);
        public Task DeleteManyProducts(IEnumerable<ProductWithoutIdDto> productDtos);
    }
}