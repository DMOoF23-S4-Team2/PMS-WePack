using System;
using AutoMapper;
using PMS.Application.DTOs.Product;
using PMS.Application.Mapper;
using PMS.Core.Entities;

namespace PMS.Application.Test.Mapping;

public class MappingTest
{
    private readonly IMapper _mapper;

    public MappingTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<DtoMapper>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void ProductMapping_ToProductDto_ShouldBeCorrect()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 10.0f
        };

        //Act
        var productDto = _mapper.Map<ProductDto>(product);

        //Assert
        Assert.Equal(1, productDto.Id);
        Assert.Equal("Test Product", productDto.Name);
        Assert.Equal("Test Description", productDto.Description);
        Assert.Equal(10.0f, productDto.Price);
    }
}


