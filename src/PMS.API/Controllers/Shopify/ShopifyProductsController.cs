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
            var products = await _shopifyProductService.GetAllShopifyProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
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
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var addedProduct = await _shopifyProductService.AddShopifyProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.Id }, addedProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product ID mismatch");
            }

            await _shopifyProductService.UpdateShopifyProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = new Product { Id = id };
            await _shopifyProductService.DeleteShopifyProduct(product);
            return NoContent();
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddManyProducts([FromBody] IEnumerable<Product> products)
        {
            await _shopifyProductService.AddManyShopifyProducts(products);
            return NoContent();
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateManyProducts([FromBody] IEnumerable<Product> products)
        {
            await _shopifyProductService.UpdateManyShopifyProducts(products);
            return NoContent();
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteManyProducts([FromBody] IEnumerable<Product> products)
        {
            await _shopifyProductService.DeleteManyShopifyProducts(products);
            return NoContent();
        }
    }
}