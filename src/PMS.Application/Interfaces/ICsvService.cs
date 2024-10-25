using System;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Interfaces
{
    public interface ICsvService
    {
        Task AddManyProductsFromCsv(string filepath);
    }
}


