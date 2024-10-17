using System;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Interfaces
{
    public interface ICsvService
    {
        List<ProductDto> GetProductCsv(string filepath);
    }
}


