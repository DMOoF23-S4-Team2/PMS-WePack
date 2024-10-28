using PMS.Application.Interfaces;
using PMS.Core.Entities;
using PMS.Core.Interfaces;
using System.Collections.Concurrent;

namespace PMS.Application.Services
{
    public class ShopifyProductService : IShopifyProductService
    {
        private readonly IShopifyProductRepository _shopifyProductRepository;

        public ShopifyProductService(IShopifyProductRepository shopifyProductRepository)
        {
            _shopifyProductRepository = shopifyProductRepository;
        }

        public async Task AddManyShopifyProducts(IEnumerable<Product> products)
        {
            await _shopifyProductRepository.AddManyProductsAsync(products);
        }

        public async Task<Product> AddShopifyProduct(Product product)
        {
            return await _shopifyProductRepository.AddProductAsync(product);
        }

        public async Task DeleteManyShopifyProducts(IEnumerable<Product> products)
        {
            await _shopifyProductRepository.DeleteManyProductsAsync(products);
        }

        public async Task DeleteShopifyProduct(Product product)
        {
            await _shopifyProductRepository.DeleteProductAsync(product);
        }

        public async Task<IReadOnlyList<Product>> GetAllShopifyProducts()
        {
            return await _shopifyProductRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetShopifyProductById(int id)
        {
            return await _shopifyProductRepository.GetProductByIdAsync(id);
        }

        public async Task UpdateManyShopifyProducts(IEnumerable<Product> products)
        {
            await _shopifyProductRepository.UpdateManyProductsAsync(products);
        }

        public async Task UpdateShopifyProduct(Product product)
        {
            await _shopifyProductRepository.UpdateProductAsync(product);
        }
    }
}
