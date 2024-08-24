
using PMS.Core.Entities;

namespace PMS.Core.Test;

public class CategoryUnitTest
{
    [Fact]
    public void Category_Properties_ShouldBeEmpty()
    {
        // Arrange
        var category = new Category();

        // Act
        var id = category.Id;
        var name = category.Name;
        var description = category.Description;
        var bottomDescription = category.BottomDescription;

        // Assert
        Assert.Equal(0, id);
        Assert.Equal(string.Empty, name);
        Assert.Equal(string.Empty, description);
        Assert.Equal(string.Empty, bottomDescription);

    }
}