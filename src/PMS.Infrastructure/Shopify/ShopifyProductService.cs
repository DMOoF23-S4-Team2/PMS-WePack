using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using PMS.Core.Entities;
using PMS.Core.Interfaces;
using Microsoft.Extensions.Configuration;


namespace PMS.Infrastructure.Shopify
{
    public class ShopifyProductService : IShopifyProductService
  {
    private readonly HttpClient _httpClient;
    private readonly string _shopifyApiUrl;
    private readonly string _accessToken;

    public ShopifyProductService(HttpClient httpClient, IConfiguration configuration, SecretClient? secretClient = null)
    {
      _httpClient = httpClient;
      _shopifyApiUrl = configuration["ShopifyApiUrl"] ?? throw new ArgumentNullException("ShopifyApiUrl configuration is missing.");

      var keyVaultUrl = configuration["KeyVaultUri"] ?? throw new ArgumentNullException("KeyVaultUri configuration is missing.");
      secretClient ??= new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

      _accessToken = GetSecretFromKeyVault(secretClient, "DevStrapAccessToken").Result;
    }

    public async Task<Product> AddProductAsync(Product product)
    {
      var mutation = ConstructProductMutation(product, "productCreate");

      var content = new StringContent(JsonSerializer.Serialize(new { query = mutation }), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
      response.EnsureSuccessStatusCode();

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
      response.EnsureSuccessStatusCode();

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
      var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
      response.EnsureSuccessStatusCode();

      var result = await response.Content.ReadAsStringAsync();
      var jsonDocument = JsonDocument.Parse(result);

      var productList = new List<Product>();

      // Navigate through the JSON structure to retrieve products
      var productsNode = jsonDocument.RootElement
        .GetProperty("data")
        .GetProperty("products")
        .GetProperty("edges");

      foreach (var productEdge in productsNode.EnumerateArray())
      {
        var productNode = productEdge.GetProperty("node");
        var productData = JsonNode.Parse(productNode.GetRawText()) ?? throw new Exception("Error parsing product data");

        var product = MapProduct(productData);
        productList.Add(product);
      }

      return productList.AsReadOnly();
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
    private static async Task<string> GetSecretFromKeyVault(SecretClient secretClient, string secretName)
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
        }}";
      }
      else
      {
        return @"
        {
          products(first: 100) {
            edges {
              node {
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
              }
            }
          }
        }";
      }
    }

    private Product MapProduct(JsonNode productData)
    {
      return new Product
      {
        Id = int.Parse(productData["id"]?.ToString() ?? "0"),
        Name = productData["title"]?.ToString() ?? string.Empty,
        Description = productData["descriptionHtml"]?.ToString() ?? string.Empty,
        Sku = productData["sku"]?.ToString() ?? string.Empty,
        Ean = productData["ean"]?.ToString() ?? string.Empty,
        Color = productData["color"]?.ToString() ?? string.Empty,
        Material = productData["material"]?.ToString() ?? string.Empty,
        ProductType = productData["productType"]?.ToString() ?? string.Empty,
        ProductGroup = productData["productGroup"]?.ToString() ?? string.Empty,
        Supplier = productData["supplier"]?.ToString() ?? string.Empty,
        SupplierSku = productData["supplierSku"]?.ToString() ?? string.Empty,
        TemplateNo = int.Parse(productData["templateNo"]?.ToString() ?? "0"),
        List = int.Parse(productData["list"]?.ToString() ?? "0"),
        Weight = float.Parse(productData["weight"]?.ToString() ?? "0"),
        Cost = float.Parse(productData["cost"]?.ToString() ?? "0"),
        Currency = productData["currency"]?.ToString() ?? string.Empty,
        Price = float.Parse(productData["price"]?.ToString() ?? "0"),
        SpecialPrice = float.Parse(productData["specialPrice"]?.ToString() ?? "0")
      };
    }

  }
}
 