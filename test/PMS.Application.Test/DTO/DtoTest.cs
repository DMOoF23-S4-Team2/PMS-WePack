using System;
using PMS.Application.DTO_s.Category;
using PMS.Application.DTO_s.Product;
namespace PMS.Application.Test.DTO;

public class DtoTest
{
    [Fact]
    public void Category_Name_ShouldBeEmpty()
    {
        //Arrange
        var category = new CategoryDto();

        //Act
        var name = category.Name;

        //Assert
        Assert.Equal(string.Empty, name);

    }

    [Fact]
    public void Product_Description_ShouldBeAssignedCorrectly()
    {
        //Arrange
        var product = new ProductDto();
        product.Description = "Test Description";

        //Act
        var description = product.Description;

        //Assert
        Assert.Equal("Test Description", description);

    }
}