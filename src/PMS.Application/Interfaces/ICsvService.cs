using System;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Interfaces
{
    public interface ICsvService
    {
        Task CreateProduct(string filepath);
        // Task DeleteProduct(string filepath);
        // Task GetProduct(string filepath);
        // Task GetProducts();
        // Task UpdateProduct(string filepath);
        Task AddManyProductsFromCsv(string filepath);
        // Task UpdateManyProducts(string filepath);
        // Task DeleteManyProducts(string filepath);
    }
}


