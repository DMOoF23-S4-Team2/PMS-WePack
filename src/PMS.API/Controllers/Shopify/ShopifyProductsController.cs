using Microsoft.AspNetCore.Mvc;
using PMS.Application.Interfaces;
using PMS.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMS.API.Controllers.Shopify
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopifyProductsController : ControllerBase
    {
        private readonly ILogger<ShopifyProductsController> _logger;
        private readonly IShopifyProductService _shopifyProductService;

        public ShopifyProductsController(ILogger<ShopifyProductsController> logger, IShopifyProductService shopifyProductService)
        {
            _logger = logger;
            _shopifyProductService = shopifyProductService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _shopifyProductService.GetAllShopifyProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                var product = await _shopifyProductService.GetShopifyProductById(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Product not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                var addedProduct = await _shopifyProductService.AddShopifyProduct(product);
                return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.Id }, addedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch");
            }

            try
            {
                await _shopifyProductService.UpdateShopifyProduct(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = new Product { Id = id };
                await _shopifyProductService.DeleteShopifyProduct(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddManyProducts([FromBody] IEnumerable<Product> products)
        {
            try
            {
                await _shopifyProductService.AddManyShopifyProducts(products);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding many products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateManyProducts([FromBody] IEnumerable<Product> products)
        {
            try
            {
                await _shopifyProductService.UpdateManyShopifyProducts(products);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating many products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteManyProducts([FromBody] IEnumerable<Product> products)
        {
            try
            {
                await _shopifyProductService.DeleteManyShopifyProducts(products);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting many products");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}