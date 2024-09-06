using System;
using AutoMapper;
using PMS.Application.DTO_s.Category;
using PMS.Application.DTO_s.Product;
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
