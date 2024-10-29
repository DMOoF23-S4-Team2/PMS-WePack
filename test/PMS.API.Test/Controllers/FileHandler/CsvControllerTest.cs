using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Application.Interfaces;
using Xunit;
using PMS.API.Controllers;


namespace PMS.API.Test{
    public class CsvControllerTest{
        
        [Fact]
        public  async Task TestUploadCsvWithEmptyFilePath()
        {
            // Arrange
            var mockCsvService = new Mock<ICsvService>();
            var controller = new CsvController(mockCsvService.Object);
            // Act
            var result = await controller.UploadCsv(string.Empty);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task TestUploadCsvWithFilePath()
        {
            // Arrange
            var mockCsvService = new Mock<ICsvService>();
            
            // Mock the response of DetermineMethod to return a success message
            mockCsvService
                .Setup(service => service.DetermineMethod(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var controller = new CsvController(mockCsvService.Object);
            
            // Act
            var result = await controller.UploadCsv("test-filepath.csv");
            // Assert
            var okResult = Assert.IsType<OkResult>(result);         
            Assert.Equal(200, okResult.StatusCode);

            // Verify that DetermineMethod was called with the correct filepath
            mockCsvService.Verify(service => service.DetermineMethod("test-filepath.csv"), Times.Once);                                
        }
    }
}