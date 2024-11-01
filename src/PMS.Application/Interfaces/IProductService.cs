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
        public Task<ProductDto> GetProduct(int id);
        public Task<ProductWithoutIdDto> CreateProduct(ProductWithoutIdDto productDto);
        public Task UpdateProduct(int id, ProductDto productDto);
        public Task DeleteProduct(int id);
        public Task AddManyProducts(IEnumerable<ProductWithoutIdDto> productDtos);
        public Task UpdateManyProducts(IEnumerable<ProductDto> productDtos);
        public Task DeleteManyProducts(IEnumerable<ProductDto> productDtos);
    }
}