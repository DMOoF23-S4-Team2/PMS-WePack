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
        CreateMap<Category, CategoryWithoutIdDto>().ReverseMap();
        CreateMap<Category, CategoryWithProductsDto>().ReverseMap();

        // Product Mapping
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductWithoutIdDto>().ReverseMap();
        CreateMap<Product, ProductWithCategoriesDto>().ReverseMap();
    }

}
