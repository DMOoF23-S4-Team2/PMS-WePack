using System.Diagnostics;
using System.Text.Json; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs.Category;
using PMS.Application.Interfaces;

namespace PMS.API.Controllers
{    
    [Route("api/[controller]")]    
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryWithoutIdDto categoryDto){
            try {                                
                var category = await categoryService.CreateCategory(categoryDto);
                return Ok(category);                        
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id){            
            try {
                var category = await categoryService.GetCategory(id);            
                if (category == null){
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {            
            try {
                var categories = await categoryService.GetCategories();
                if (categories == null)
                {
                    return NotFound("No categories found.");
                }
                return Ok(categories);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id) {
            try {
                await categoryService.DeleteCategory(id);
                return NoContent();
            }
            catch (ArgumentNullException){
                return NotFound();
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryWithoutIdDto categoryDto)
        {                        
            try {
                await categoryService.UpdateCategory(id, categoryDto);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        } 
    }
}
