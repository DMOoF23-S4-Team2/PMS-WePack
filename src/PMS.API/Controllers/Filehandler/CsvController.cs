using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Interfaces;

namespace PMS.API.Controllers
{    
    [Route("api/[controller]")]    
    [ApiController]
    public class CsvController : ControllerBase
    {
        private readonly ICsvService _csvService;
        public CsvController(ICsvService csvService){
            _csvService = csvService;
        }         

        [HttpPost("upload-csv")]        
        public async Task<IActionResult> UploadCsv(string filepath)
        {            
            if (string.IsNullOrWhiteSpace(filepath)) {                
                return BadRequest("File path is required.");
            }
            
            try {                                                     
                await _csvService.DetermineMethod(filepath);
                return Ok();
            }
            catch (Exception ex) {                
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }

        // For development purposes. Will be deleted when Adam is saying "GO!"
        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] string filepath)
        {            
            if (string.IsNullOrWhiteSpace(filepath)) {
                return BadRequest("File path is required.");
            }
            
            try {                                
                await _csvService.CreateProduct(filepath);
                return Ok("Product created successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }

        [HttpDelete("delete-product")]
        public async Task<IActionResult> DeleteProduct([FromBody] string filepath)
        {            
            if (string.IsNullOrWhiteSpace(filepath)) {
                return BadRequest("File path is required.");
            }
            
            try {                                
                await _csvService.DeleteProduct(filepath);
                return Ok("Product deleted successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }

        [HttpPost("add-many-products")]
        public async Task<IActionResult> AddManyProducts([FromBody] string filepath)
        {            
            if (string.IsNullOrWhiteSpace(filepath)) {
                return BadRequest("File path is required.");
            }
            
            try {                                
                await _csvService.AddManyProducts(filepath);
                return Ok("Product created successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }

        [HttpDelete("delete-many-products")]
        public async Task<IActionResult> DeleteManyProducts([FromBody] string filepath)
        {            
            if (string.IsNullOrWhiteSpace(filepath)) {
                return BadRequest("File path is required.");
            }
            
            try {                                
                await _csvService.DeleteManyProducts(filepath);
                return Ok("Product created successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }

        [HttpPost("update-many-product")]
        public async Task<IActionResult> UpdateManyProducts([FromBody] string filepath)
        {            
            if (string.IsNullOrWhiteSpace(filepath)) {
                return BadRequest("File path is required.");
            }
            
            try {                                
                await _csvService.UpdateManyProducts(filepath);
                return Ok("Product created successfully.");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }        


    }
}