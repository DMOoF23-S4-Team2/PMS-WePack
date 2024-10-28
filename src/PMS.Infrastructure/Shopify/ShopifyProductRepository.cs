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
    //!SECTION
    public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
    {
      var query = @"
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

      using var httpClient = new HttpClient();
      var content = new StringContent(JsonSerializer.Serialize(new { query }), Encoding.UTF8, "application/json");
      httpClient.DefaultRequestHeaders.Add("X-Shopify-Access-Token", _accessToken);
      var response = await httpClient.PostAsync(_shopifyApiUrl, content);

      if (!response.IsSuccessStatusCode)
      {
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new Exception($"Request failed with status code {response.StatusCode}: {errorContent}");
      }

      try
      {
        var result = await response.Content.ReadAsStringAsync();
        var json = JsonNode.Parse(result);
        var productsNode = json?["data"]?["products"]?["edges"];

        if (productsNode == null)
        {
          throw new Exception($"Products data not found. Full response: {result}");
        }

        var products = new List<Product>();
        foreach (var edge in productsNode.AsArray())
        {
          var node = edge["node"];

          var product = new Product
          {
            Name = node["title"].ToString(),
            Description = node["descriptionHtml"].ToString(),
            Supplier = node["vendor"].ToString(),
            ProductType = node["productType"].ToString(),
            ProductGroup = node["tags"].ToString(),

            // Variants

            //Id = int.Parse(node["variants"]["edges"][0]["node"]["id"].ToString()),
            Sku = node["variants"]["edges"][0]["node"]["sku"].ToString(),
            Weight = float.Parse(node["variants"]["edges"][0]["node"]["weight"]?.ToString() ?? "0", CultureInfo.InvariantCulture),
            Ean = node["variants"]["edges"][0]["node"]["barcode"].ToString(),

            SpecialPrice = node["variants"]["edges"][0]["node"]["compareAtPrice"] != null
        ? float.Parse(node["variants"]["edges"][0]["node"]["compareAtPrice"].ToString() ?? "0", CultureInfo.InvariantCulture)
        : 0,

            Price = float.Parse(node["variants"]["edges"][0]["node"]["contextualPricing"]["price"]["amount"]?.ToString() ?? "0", CultureInfo.InvariantCulture),
            Currency = node["variants"]["edges"][0]["node"]["contextualPricing"]["price"]["currencyCode"].ToString(),

            Color = node["variants"]["edges"][0]["node"]["selectedOptions"]
              .AsArray()
              .FirstOrDefault(option => option["name"]?.ToString() == "Farve")?["value"]?.ToString() ?? string.Empty,

            // Custom fields
            Material = node["metafields"]["edges"]
                .AsArray()
                .FirstOrDefault(edge => edge["node"]?["key"]?.ToString() == "multiple_material")?["node"]?["value"]?.ToString() ?? string.Empty,
            TemplateNo = int.TryParse(node["metafields"]["edges"]
                .AsArray()
                .FirstOrDefault(edge => edge["node"]?["key"]?.ToString() == "template_number")?["node"]?["value"]?.ToString(), out var templateNoValue) ? templateNoValue : 0,
            List = int.TryParse(node["metafields"]["edges"]
                .AsArray()
                .FirstOrDefault(edge => edge["node"]?["key"]?.ToString() == "week_list")?["node"]?["value"]?.ToString(), out var listValue) ? listValue : 0,

            //! Not working
            Cost = float.Parse(node["variants"]["edges"][0]["node"]["selectedOptions"]
            .AsArray()
            .FirstOrDefault(option => option["name"]?.ToString() == "cost")?["value"]?.ToString() ?? "0"),

            SupplierSku = node["metafields"]["edges"]
                .AsArray()
                .FirstOrDefault(edge => edge["node"]?["key"]?.ToString() == "supplier_sku")?["node"]?["value"]?.ToString() ?? string.Empty,

          };

          products.Add(product);
        }

        return products.AsReadOnly();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error parsing products data: {ex.Message}");
        throw;
      }
    }
    //!SECTION


    //FIXME - Commented out Only for testing purposes
    // public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
    // {
    //   var query = ConstructProductQuery();

    //   var content = new StringContent(JsonSerializer.Serialize(new { query }), Encoding.UTF8, "application/json");

    //   var response = await _httpClient.PostAsync(_shopifyApiUrl, content);

    //   if (!response.IsSuccessStatusCode)
    //   {
    //     var errorContent = await response.Content.ReadAsStringAsync();
    //     throw new Exception($"Request failed with status code {response.StatusCode}: {errorContent}");
    //   }

    //   var result = await response.Content.ReadAsStringAsync();
    //   var jsonDocument = JsonDocument.Parse(result);

    //   var productList = new List<Product>();

    //   // Navigate through the JSON structure to retrieve products
    //   var productsNode = jsonDocument.RootElement
    //     .GetProperty("data")
    //     .GetProperty("products")
    //     .GetProperty("edges");

    //   foreach (var productEdge in productsNode.EnumerateArray())
    //   {
    //     var productNode = productEdge.GetProperty("node");
    //     var productData = JsonNode.Parse(productNode.GetRawText()) ?? throw new Exception("Error parsing product data");

    //     var product = MapProduct(productData);
    //     productList.Add(product);
    //   }

    //   return productList.AsReadOnly();
    // }

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
          id
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
              priceV2 {
              amount
              currencyCode
              }
              compareAtPriceV2 {
              amount
              currencyCode
              }
              cost
              barcode
            }
            }
          }
          metafields(first: 10) {
            edges {
            node {
              namespace
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
      // Extract the first variant if available
      var variantNode = productData["variants"]?["edges"]?[0]?["node"];

      // Extract metafields
      var metafieldsArray = productData["metafields"]?["edges"]?.AsArray();

      // Initialize an empty dictionary to store metafields for easier access
      var metafieldsDictionary = new Dictionary<string, string>();

      if (metafieldsArray != null)
      {
        foreach (var metafieldNode in metafieldsArray)
        {
          var key = metafieldNode?["node"]?["key"]?.ToString();
          var value = metafieldNode?["node"]?["value"]?.ToString();
          if (key != null && value != null)
          {
            metafieldsDictionary[key] = value;
          }
        }
      }

      return new Product
      {
        Id = int.Parse(productData["id"]?.ToString() ?? "0"),
        Name = productData["title"]?.ToString() ?? string.Empty,
        Description = productData["descriptionHtml"]?.ToString() ?? string.Empty,
        ProductType = productData["productType"]?.ToString() ?? string.Empty,
        ProductGroup = productData["tags"] != null ? string.Join(",", productData["tags"].AsArray()) : string.Empty, // Assuming productGroup is in tags
        Supplier = productData["vendor"]?.ToString() ?? string.Empty,

        // Variant-specific data
        Sku = variantNode?["sku"]?.ToString() ?? string.Empty,
        Weight = float.Parse(variantNode?["weight"]?.ToString() ?? "0"),
        Cost = float.Parse(variantNode?["cost"]?.ToString() ?? "0"),
        Ean = variantNode?["barcode"]?.ToString() ?? string.Empty, // Assuming barcode is used for EAN

        // Price and currency data
        Price = float.Parse(variantNode?["priceV2"]?["amount"]?.ToString() ?? "0"),
        SpecialPrice = float.Parse(variantNode?["compareAtPriceV2"]?["amount"]?.ToString() ?? "0"),
        Currency = variantNode?["priceV2"]?["currencyCode"]?.ToString() ?? string.Empty,

        // Custom fields via metafields
        Color = metafieldsDictionary.ContainsKey("color") ? metafieldsDictionary["color"] : string.Empty,
        Material = metafieldsDictionary.ContainsKey("material") ? metafieldsDictionary["material"] : string.Empty,
        SupplierSku = metafieldsDictionary.ContainsKey("supplierSku") ? metafieldsDictionary["supplierSku"] : string.Empty,
        TemplateNo = metafieldsDictionary.ContainsKey("templateNo") ? int.Parse(metafieldsDictionary["templateNo"]) : 0,
        List = metafieldsDictionary.ContainsKey("list") ? int.Parse(metafieldsDictionary["list"]) : 0
      };
    }

  }
}
