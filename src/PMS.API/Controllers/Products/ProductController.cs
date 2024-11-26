using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs.Product;
using PMS.Application.Interfaces;
using PMS.Core.Entities;

namespace PMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase{    
    private readonly IProductService productService;

    public ProductController(IProductService productService){
        this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductWithoutIdDto productDto){
        try {                                
            var product = await productService.CreateProduct(productDto);
            return Ok(product);                        
        }
        catch (Exception ex) {
            return StatusCode(500, $"Error: {ex.Message}");
        } 
    }

    [HttpPost("addMany")]
    public async Task<IActionResult> AddManyProducts([FromBody] IEnumerable<ProductWithoutIdDto> productDtos)
    {
        try
        {
            await productService.AddManyProducts(productDtos);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpDelete("{sku}")]
    public async Task<IActionResult> DeleteProduct(string sku) {
        try {
            await productService.DeleteProduct(sku);
            return NoContent();
        }
        catch (ArgumentNullException){
            return NotFound();
        }
        catch (Exception ex) {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpDelete("deleteMany")]
    public async Task<IActionResult> DeleteManyProducts([FromBody] IEnumerable<ProductWithoutIdDto> dtoList)
    {
        try
        {
            await productService.DeleteManyProducts(dtoList);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id){
        try {
            var product = await productService.GetProduct(id);
            if (product == null){
                return NotFound();
            }
            return Ok(product);
        }
        catch (Exception ex) {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        try {
            var products = await productService.GetProducts();
            if (products == null)
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }
        catch (Exception ex) {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
    {                        
        try {
            await productService.UpdateProduct(id, productDto);
            return NoContent();
        }
        catch (ArgumentNullException)
        {
            return NotFound($"Product with Id {id} not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }  

    [HttpPut("updateMany")]
    public async Task<IActionResult> UpdateManyProducts([FromBody] IEnumerable<ProductDto> productDtos)
    {
        try
        {
            await productService.UpdateManyProducts(productDtos);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
