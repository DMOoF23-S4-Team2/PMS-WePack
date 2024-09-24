using System;
using AutoMapper;
using PMS.Application.DTOs.Category;
using PMS.Application.DTOs.Product;
using PMS.Core.Entities;

namespace PMS.Application.Mapper;

public class DtoMapper : Profile
{
    public DtoMapper()
    {
        // Category Mapping
        CreateMap<Category, CategoryDto>().ReverseMap();

        // Product Mapping
        CreateMap<Product, ProductDto>().ReverseMap();
    }

}
