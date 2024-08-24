using System;
using PMS.Core.Entities;

namespace PMS.Core.Test;

public class ProductUnitTest
{
    [Fact]
    public void Product_Properties_Should_Be_Assigned_Correctly()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Product 1",
        };

        // Act & Assert
        Assert.Equal(1, product.Id);
        Assert.Equal("Product 1", product.Name);


    }

}
