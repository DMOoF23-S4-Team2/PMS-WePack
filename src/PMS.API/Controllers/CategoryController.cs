using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs.Category;
using PMS.Application.Interfaces;

namespace PMS.API.Controllers;

[Route("pms_api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase {    
    
    private readonly ICategoryService? categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    // POST: New Category
    // [HttpPost]
    // public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto){
    //     var category = categoryService.CreateCategory(categoryDto);        
    // }

    // DELETE: Delete Category from ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id) {
        try {
            await categoryService.DeleteCategory(id);
        }
        catch (ArgumentNullException){
            return NotFound();
        }
        return NoContent();
    }
    
    // GET: Categories
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await categoryService.GetCategories();
        return Ok(categories);
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
    
    // PUT: Update Category from ID
    // [HttpPut("{id}")]
    // public async Task<IActionResult UpdateCategory(int id){
    //     return;
    // }
}