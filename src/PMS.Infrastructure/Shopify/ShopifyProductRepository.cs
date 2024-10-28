using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using PMS.Core.Entities;
using PMS.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Globalization;


namespace PMS.Infrastructure.Shopify
{
  public class ShopifyProductRepository : IShopifyProductRepository
  {
    private readonly HttpClient _httpClient;
    private readonly string _shopifyApiUrl;
    private readonly string _accessToken;
    private readonly string _apiKey;


    public ShopifyProductRepository(HttpClient httpClient, IConfiguration configuration, SecretClient? secretClient = null)
    {
      _httpClient = httpClient;
      _shopifyApiUrl = configuration["ShopifyApiUrl"] ?? throw new ArgumentNullException("ShopifyApiUrl configuration is missing.");

      var keyVaultUrl = configuration["KeyVaultUri"] ?? throw new ArgumentNullException("KeyVaultUri configuration is missing.");
      secretClient ??= new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

      _apiKey = GetSecretFromKeyVault(secretClient, "DevStrapApiKey").Result;

      _accessToken = GetSecretFromKeyVault(secretClient, "DevStrapAccessToken").Result;
    }

    public async Task<Product> AddProductAsync(Product product)
    {
      var mutation = ConstructProductMutation(product, "productCreate");

      var content = new StringContent(JsonSerializer.Serialize(new { query = mutation }), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
      if (!response.IsSuccessStatusCode)
      {
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new Exception($"Request failed with status code {response.StatusCode}: {errorContent}");
      }

      var result = await response.Content.ReadAsStringAsync();
      var json = JsonNode.Parse(result);
      var dataNode = json?["data"] ?? throw new Exception("Data not found");
      var productCreateNode = dataNode["productCreate"] ?? throw new Exception("Product creation data not found");
      var productData = productCreateNode["product"] ?? throw new Exception("Error creating product");

      return MapProduct(productData);
    }

    // Create multiple products
    public async Task AddManyProductsAsync(IEnumerable<Product> products)
    {
      foreach (var product in products)
      {
        await AddProductAsync(product); // Reusing the AddProductAsync method
      }
    }

    // Get a product by ID
    public async Task<Product> GetProductByIdAsync(int id)
    {
      var query = ConstructProductQuery(id);

      var content = new StringContent(JsonSerializer.Serialize(new { query }), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
      if (!response.IsSuccessStatusCode)
      {
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new Exception($"Request failed with status code {response.StatusCode}: {errorContent}");
      }

      var result = await response.Content.ReadAsStringAsync();
      var json = JsonNode.Parse(result);
      var dataNode = json?["data"] ?? throw new Exception("Data not found");
      var productNode = dataNode["product"] ?? throw new Exception("Product not found");
      var productData = productNode;

      return MapProduct(productData);
    }

    // Get all products
    public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
    {
      var query = ConstructProductQuery();
      var content = new StringContent(JsonSerializer.Serialize(new { query }), Encoding.UTF8, "application/json");
      _httpClient.DefaultRequestHeaders.Add("X-Shopify-Access-Token", _accessToken);

      var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
      if (!response.IsSuccessStatusCode)
      {
      var errorContent = await response.Content.ReadAsStringAsync();
      throw new Exception($"Request failed with status code {response.StatusCode}: {errorContent}");
      }

      var result = await response.Content.ReadAsStringAsync();
      var json = JsonNode.Parse(result);
      var productsNode = json?["data"]?["products"]?["edges"] ?? throw new Exception("Products data not found");

      var products = productsNode.AsArray().Select(edge => MapProduct(edge["node"])).ToList();
      return products.AsReadOnly();
    }

    // Update a single product
    public async Task UpdateProductAsync(Product product)
    {
      var mutation = ConstructProductMutation(product, "productUpdate");

      var content = new StringContent(JsonSerializer.Serialize(new { query = mutation }), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
      response.EnsureSuccessStatusCode();
    }

    // Update multiple products
    public async Task UpdateManyProductsAsync(IEnumerable<Product> products)
    {
      foreach (var product in products)
      {
        await UpdateProductAsync(product); // Reusing the UpdateProductAsync method
      }
    }

    // Patch product (partial update)
    public async Task PatchProductAsync(Product product)
    {
      // Similar to update, Shopify only supports updating fields, so we use the same mutation approach
      await UpdateProductAsync(product);
    }

    // Delete a single product
    public async Task DeleteProductAsync(Product product)
    {
      var mutation = @"
          mutation {
            productDelete(input: {
              id: """ + product.Id + @"""
            }) {
              deletedProductId
            }
          }";

      var content = new StringContent(JsonSerializer.Serialize(new { query = mutation }), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
      response.EnsureSuccessStatusCode();
    }

    // Delete multiple products
    public async Task DeleteManyProductsAsync(IEnumerable<Product> products)
    {
      foreach (var product in products)
      {
        await DeleteProductAsync(product); // Reusing the DeleteProductAsync method
      }
    }

    //!SECTION Helper Methods
    // Method to retrieve a secret from Azure Key Vault
    private async Task<string> GetSecretFromKeyVault(SecretClient secretClient, string secretName)
    {
      try
      {
        KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);
        return secret.Value;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error retrieving secret {secretName} from Azure Key Vault: {ex.Message}");
        throw;
      }
    }

    private string ConstructProductMutation(Product product, string mutationType)
    {
      return $@"
    mutation {{
      {mutationType}(input: {{
        id: ""{product.Id}"",
        title: ""{product.Name}"",
        descriptionHtml: ""{product.Description}"",
        sku: ""{product.Sku}"",
        ean: ""{product.Ean}"",
        color: ""{product.Color}"",
        material: ""{product.Material}"",
        productType: ""{product.ProductType}"",
        productGroup: ""{product.ProductGroup}"",
        supplier: ""{product.Supplier}"",
        supplierSku: ""{product.SupplierSku}"",
        templateNo: {product.TemplateNo},
        list: {product.List},
        weight: {product.Weight},
        cost: {product.Cost},
        currency: ""{product.Currency}"",
        price: {product.Price},
        specialPrice: {product.SpecialPrice}
      }}) {{
        product {{
          id
          title
          descriptionHtml
          sku
          ean
          color
          material
          productType
          productGroup
          supplier
          supplierSku
          templateNo
          list
          weight
          cost
          currency
          price
          specialPrice
        }}
      }}
    }}";
    }

    private string ConstructProductQuery(int? id = null)
    {
      if (id.HasValue)
      {
      return $@"
      {{
      product(id: ""gid://shopify/Product/{id.Value}"") {{
      id
      title
      descriptionHtml
      vendor
      productType
      tags

      variants(first: 1) {{
        edges {{
        node {{
        id
        sku
        weight
        priceV2 {{
        amount
        currencyCode
        }}
        compareAtPriceV2 {{
        amount
        currencyCode
        }}
        cost
        barcode
        }}
        }}
      }}

      metafields(first: 10) {{
        edges {{
        node {{
        namespace
        key
        value
        }}
        }}
      }}
      }}
      }}";
      }
      else
      {
      return @"
    {
      products(first: 100) {
      edges {
        node {
        title
        descriptionHtml
        vendor
        productType
        tags

        variants(first: 1) {
          edges {
          node {
            id
            sku
            weight
            barcode
            compareAtPrice
            contextualPricing(context: {country: DK}) {
            price {
              amount
              currencyCode
            }
            }
            selectedOptions {
            name 
            value
            }
          }
          }
        }
        
        metafields(first: 10, namespace: ""custom""){
          edges {
          node {
            key
            value
          }
          }
        }
        }
      }
      }
    }";
      }
    }

    private Product MapProduct(JsonNode productData)
    {

      var product = new Product
      {
      Name = productData["title"]?.ToString() ?? string.Empty,
      Description = productData["descriptionHtml"]?.ToString() ?? string.Empty,
      Supplier = productData["vendor"]?.ToString() ?? string.Empty,
      ProductType = productData["productType"]?.ToString() ?? string.Empty,
      ProductGroup = productData["tags"]?.ToString() ?? string.Empty,

      // Variants
      //? Id = int.Parse(productData["variants"]?["edges"]?[0]?["node"]?["id"]?.ToString() ?? "0"),
      Sku = productData["variants"]?["edges"]?[0]?["node"]?["sku"]?.ToString() ?? string.Empty,
      Weight = float.Parse(productData["variants"]?["edges"]?[0]?["node"]?["weight"]?.ToString() ?? "0", CultureInfo.InvariantCulture),
      Ean = productData["variants"]?["edges"]?[0]?["node"]?["barcode"]?.ToString() ?? string.Empty,

      SpecialPrice = productData["variants"]?["edges"]?[0]?["node"]?["compareAtPrice"] != null
        ? float.Parse(productData["variants"]?["edges"]?[0]?["node"]?["compareAtPrice"]?.ToString() ?? "0", CultureInfo.InvariantCulture)
        : 0,

      Price = float.Parse(productData["variants"]?["edges"]?[0]?["node"]?["contextualPricing"]?["price"]?["amount"]?.ToString() ?? "0", CultureInfo.InvariantCulture),
      Currency = productData["variants"]?["edges"]?[0]?["node"]?["contextualPricing"]?["price"]?["currencyCode"]?.ToString() ?? string.Empty,

      Color = productData["variants"]?["edges"]?[0]?["node"]?["selectedOptions"]
        ?.AsArray()
        ?.FirstOrDefault(option => option?["name"]?.ToString() == "Farve")?["value"]?.ToString() ?? string.Empty,

      // Custom fields
      Material = productData["metafields"]?["edges"]
        ?.AsArray()
        ?.FirstOrDefault(edge => edge?["node"]?["key"]?.ToString() == "multiple_material")?["node"]?["value"]?.ToString() ?? string.Empty,
      TemplateNo = int.TryParse(productData["metafields"]?["edges"]
        ?.AsArray()
        ?.FirstOrDefault(edge => edge?["node"]?["key"]?.ToString() == "template_number")?["node"]?["value"]?.ToString(), out var templateNoValue) ? templateNoValue : 0,
      List = int.TryParse(productData["metafields"]?["edges"]
        ?.AsArray()
        ?.FirstOrDefault(edge => edge?["node"]?["key"]?.ToString() == "week_list")?["node"]?["value"]?.ToString(), out var listValue) ? listValue : 0,

      Cost = float.Parse(productData["variants"]?["edges"]?[0]?["node"]?["selectedOptions"]
        ?.AsArray()
        ?.FirstOrDefault(option => option?["name"]?.ToString() == "cost")?["value"]?.ToString() ?? "0"),

      SupplierSku = productData["metafields"]?["edges"]
        ?.AsArray()
        ?.FirstOrDefault(edge => edge?["node"]?["key"]?.ToString() == "supplier_sku")?["node"]?["value"]?.ToString() ?? string.Empty,
      };

      return product;
    }
  }
}