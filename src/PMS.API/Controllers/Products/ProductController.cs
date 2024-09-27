using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs.Product;
using PMS.Application.Interfaces;

namespace PMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase{    
    private readonly IProductService? productService;

    public ProductController(IProductService productService){
        this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    // POST: New Product
    // [HttpPost]
    // public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto){
    //     var product = productService.CreateProduct(productDto);
    //     return "Product created";
    // }

    // DELETE: Delete Product from ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id) {
        if (productService == null) 
        {
            return BadRequest("Service unavailable.");
        }

        try {
            await productService.DeleteProduct(id);
        }
        catch (ArgumentNullException){
            return NotFound();
        }
        return NoContent();
    }

    // GET: Single Product from ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id){
        var product = await productService.GetProduct(id);
        if (product == null){
            return NotFound();
        }
        return Ok(product);
    }

    // GET: Products
    [HttpGet("/products")]
    public async Task<IActionResult> GetProducts()
    {
        // Ensure productService is not null
        if (productService == null)
        {
            return Problem("Category service is not available.");
        }        
        
        // Get Products
        var products = await productService.GetProducts();
        
        // If products where not found
        if (products == null)
        {
            return NotFound("No products found.");
        }

        // Return if everything is awesome
        return Ok(products);
    }

    // // PUT: Update Category from ID
    // [HttpPut("{id}")]
    // public async Task<IActionResult UpdateProduct(int id){
    //     return;
    // }       
}
