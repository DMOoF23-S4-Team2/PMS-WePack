using System;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Interfaces
{
    public interface ICsvService
    {
        Task DetermineMethod(string filepath);
        Task CreateProduct(string filepath);
        Task DeleteProduct(string filepath);
        Task GetProduct(string filepath);
        Task GetProducts(string filepath);
        Task AddManyProducts(string filepath);
        Task DeleteManyProducts(string filepath);
        Task UpdateManyProducts(string filepath);
        // Task UpdateProduct(string filepath);
    }
}


