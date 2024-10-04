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

        // POST: New Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto){
            try {                                
                var category = await categoryService.CreateCategory(categoryDto);
                return Ok(category);                        
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error: {ex.Message}");
            }                           
        }

        //GET: Single Category from ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id){            
            var category = await categoryService.GetCategory(id);            
            if (category == null){
                return NotFound();
            }
            return Ok(category);
        }

        // GET: Categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {            
            // Ensure categoryService is not null
            if (categoryService == null)
            {
                return Problem("Category service is not available.");
            }
            // Getting categories
            var categories = await categoryService.GetCategories();
            
            // If categories where not found
            if (categories == null)
            {
                return NotFound("No categories found.");
            }
            
            // All good? Return!
            return Ok(categories);
        }
        
        // DELETE: Delete Category from ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id) {
            if (categoryService == null) 
            {
                return BadRequest("Service unavailable.");
            }

            try {
                await categoryService.DeleteCategory(id);
            }
            catch (ArgumentNullException){
                return NotFound();
            }
            return NoContent();
        }
        
        // PUT: Update Category from ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {                        
            try {
                // Try to update the category
                await categoryService.UpdateCategory(id, categoryDto);
                // Returns 204
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                // Return a 404 if the category was not found
                return NotFound($"Category with ID {id} not found.");
            }
            catch (Exception ex)
            {
                // Catch any other exceptions and return a 500 error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        } 
    }
}
