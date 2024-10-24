using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Core.Entities;

namespace PMS.Application.Interfaces
{
    public interface IShopifyProductService
    {
        Task<IReadOnlyList<Product>> GetAllShopifyProducts();
        Task<Product> GetShopifyProductById(int id);
        Task<Product> AddShopifyProduct(Product product);
        Task UpdateShopifyProduct(Product product);
        Task PatchShopifyProduct(Product product);
        Task DeleteShopifyProduct(Product product);
        Task AddManyShopifyProducts(IEnumerable<Product> products);
        Task UpdateManyShopifyProducts(IEnumerable<Product> products);
        Task DeleteManyShopifyProducts(IEnumerable<Product> products);
    }
}