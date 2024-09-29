using PMS.API.Controllers;
using PMS.Application.DTOs.Category;
using PMS.Application.Interfaces;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;


namespace PMS.API.Test;
public class CategoryControllerTest
{        
    private Mock<ICategoryService> mockCategoryService;
    public CategoryControllerTest(){
        mockCategoryService = new Mock<ICategoryService>();
    }
    
    // Test : Category created
    [Fact]
    public async Task PostCategory_ReturnsOkResult_WithCategory(){
        // Arrange
        var mockCategoryService = new Mock<ICategoryService>();        
        var newCategory = new CategoryDto { Id = 1, Name = "New Category" };        
        // Set up the mock service to return the newCategory (async) when CreateCategory is called
        mockCategoryService
            .Setup(service => service.CreateCategory(It.IsAny<CategoryDto>()))
            .ReturnsAsync(newCategory);
        
        // Inject the mock service into the controller
        var controller = new CategoryController(mockCategoryService.Object);
        
        // Act                        
        var result = await controller.CreateCategory(newCategory);
        
        // Assert
        // Verifies the result is OkObjectResult
        var okResult = Assert.IsType<OkObjectResult>(result);
        // Verifies the correct object type and that it is not null
        var returnedCategory = Assert.IsType<CategoryDto>(okResult.Value);
        // The newCategory ID should match the returned category ID
        Assert.Equal(newCategory.Id, returnedCategory.Id);       
    }

    // Test Get Category (Single)
    // Test : GetCategory returns Ok with the correct category when found
    [Fact]
    public async Task GetCategory_ReturnsOkResult_WithCategory_WhenCategoryExists()
    {
        // Arrange         
        var sampleCategoryId = 1;       
        var sampleCategory = new CategoryDto { Id = sampleCategoryId, Name = "Electronics" };
        // Setup the mock to return the sample category when the given ID is requested
        mockCategoryService.Setup(service => service.GetCategory(sampleCategoryId)).ReturnsAsync(sampleCategory);
        // Inject the mock service into the controller
        var controller = new CategoryController(mockCategoryService.Object);
        
        // Act
        var result = await controller.GetCategory(sampleCategoryId);
        
        // Assert
        // Verifies the result is OkObjectResult
        var okResult = Assert.IsType<OkObjectResult>(result); 
        // Verifies the correct object type
        var returnedCategory = Assert.IsType<CategoryDto>(okResult.Value); 
        // Checks that the returned category ID matches the expected ID
        Assert.Equal(sampleCategoryId, returnedCategory.Id); 
        // Checks that the name matches the expected value        
        Assert.Equal("Electronics", returnedCategory.Name); 
    }

    // Test returns NotFound when no category is found
    [Fact]
    public async Task GetCategory_ReturnsNotFoundResult_WhenCategoryNotExists(){        
        // Arrange        
        var categoryId = 1;
        // Inject the mock service into the controller
        var controller = new CategoryController(mockCategoryService.Object);
        
        // Act
        var result = await controller.GetCategory(categoryId);
        
        // Assert        
        Assert.IsType<NotFoundResult>(result);
    }
 
    // Test Get All Categories
    [Fact]
    public async Task GetCategories_ReturnOkResult_WithCategories_WhenCategoriesExists(){
        // Arrange                 
        // A list of categories to simuate the respone
        var categories = new List<CategoryDto>{
            new CategoryDto {Id = 1, Name = "First new category"},
            new CategoryDto {Id = 2, Name = "Second new category"},
        };
        // Setup to return the list async
        mockCategoryService.Setup(service => service.GetCategories()).ReturnsAsync(categories);
        // Inject the controller
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.GetCategories();

        // Assert
        // Expect a okresultobject (status code: 200)
        var okResult = Assert.IsType<OkObjectResult>(result);
        // Expect a list of categoryDtos
        var returnCategories = Assert.IsType<List<CategoryDto>>(okResult.Value);
        // The number of categories return should be the same length as our test list
        Assert.Equal(categories.Count, returnCategories.Count);
    }

    [Fact]
    public async Task GetCategories_ReturnNotFound_WhenNoCategoriesExists(){
        // Arrange
        mockCategoryService.Setup(service => service.GetCategories()).ReturnsAsync((List<CategoryDto>)null);        
        // Inject the controller
        var controller = new CategoryController(mockCategoryService.Object);
        // Act
        var result = await controller.GetCategories();
        // Assert
        // Not Found
        Assert.IsType<NotFoundObjectResult>(result);
    
    }

    // Test Delete Category
    [Fact]
    public async Task DeleteCategory_ReturnNoContent_WhenCategoryExists(){
        // Arrange
        var sampleCategoryId = 1; // Assume we are deleting category with ID 1

        // Setup the mock to successfully delete the category (i.e., Task completes)
        mockCategoryService
            .Setup(service => service.DeleteCategory(sampleCategoryId))
            .Returns(Task.CompletedTask); // Simulating successful deletion without return

        // Inject the mock service into the controller
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.DeleteCategory(sampleCategoryId);

        // Assert: Ensure that the result is NoContent
        Assert.IsType<NoContentResult>(result);

        // Verify that the DeleteCategory method was called with the correct ID
        mockCategoryService.Verify(service => service.DeleteCategory(sampleCategoryId), Times.Once);
    }
    [Fact]
    public async Task DeleteCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var nonExistentCategoryId = 999; // Assume this category doesn't exist

        // Setup the mock to throw an exception when trying to delete a non-existing category
        mockCategoryService
            .Setup(service => service.DeleteCategory(nonExistentCategoryId))
            .ThrowsAsync(new ArgumentNullException()); // Simulate a not-found exception

        // Inject the mock service into the controller
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.DeleteCategory(nonExistentCategoryId);

        // Assert: Ensure the result is NotFound
        Assert.IsType<NotFoundResult>(result);

        // Verify that DeleteCategory was called once with the given ID
        mockCategoryService.Verify(service => service.DeleteCategory(nonExistentCategoryId), Times.Once);
    }   
}