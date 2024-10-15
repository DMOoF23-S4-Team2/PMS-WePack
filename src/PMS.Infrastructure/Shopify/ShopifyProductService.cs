using System.Net.Http;
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

  public ShopifyProductService(HttpClient httpClient, IConfiguration configuration)
  {
    _httpClient = httpClient;

    // Reading from appsettings.json via IConfiguration
    _shopifyApiUrl = configuration["Shopify:ApiUrl"] ?? 
      throw new ArgumentNullException("Shopify:ApiUrl is missing in appsettings.json");
    _accessToken = configuration["Shopify:AccessToken"] ?? 
      throw new ArgumentNullException("Shopify:AccessToken is missing in appsettings.json");

    _httpClient.DefaultRequestHeaders.Add("X-Shopify-Access-Token", _accessToken);
  }

  // Create a single product
  public async Task<Product> AddProductAsync(Product product)
  {
    var mutation = @"
      mutation {
        productCreate(input: {
        title: """ + product.Name + @""",
        descriptionHtml: """ + product.Description + @""",
        sku: """ + product.Sku + @""",
        ean: """ + product.Ean + @""",
        color: """ + product.Color + @""",
        material: """ + product.Material + @""",
        productType: """ + product.ProductType + @""",
        productGroup: """ + product.ProductGroup + @""",
        supplier: """ + product.Supplier + @""",
        supplierSku: """ + product.SupplierSku + @""",
        templateNo: " + product.TemplateNo + @",
        list: " + product.List + @",
        weight: " + product.Weight + @",
        cost: " + product.Cost + @",
        currency: """ + product.Currency + @""",
        price: " + product.Price + @",
        specialPrice: " + product.SpecialPrice + @"
        }) {
        product {
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
      }";

    var content = new StringContent(JsonSerializer.Serialize(new { query = mutation }), Encoding.UTF8, "application/json");
    var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
    response.EnsureSuccessStatusCode();

    var result = await response.Content.ReadAsStringAsync();
    var json = JsonNode.Parse(result);
    var productData = json["data"]["productCreate"]["product"];

    return new Product
    {
      Id = int.Parse(productData["id"].ToString()),
      Name = productData["title"].ToString(),
      Description = productData["descriptionHtml"].ToString(),
      Sku = productData["sku"].ToString(),
      Ean = productData["ean"].ToString(),
      Color = productData["color"].ToString(),
      Material = productData["material"].ToString(),
      ProductType = productData["productType"].ToString(),
      ProductGroup = productData["productGroup"].ToString(),
      Supplier = productData["supplier"].ToString(),
      SupplierSku = productData["supplierSku"].ToString(),
      TemplateNo = int.Parse(productData["templateNo"].ToString()),
      List = int.Parse(productData["list"].ToString()),
      Weight = float.Parse(productData["weight"].ToString()),
      Cost = float.Parse(productData["cost"].ToString()),
      Currency = productData["currency"].ToString(),
      Price = float.Parse(productData["price"].ToString()),
      SpecialPrice = float.Parse(productData["specialPrice"].ToString())
    };
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
      var query = @"
    {
      product(id: ""gid://shopify/Product/" + id + @""") {
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
    }";

      var content = new StringContent(JsonSerializer.Serialize(new { query }), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(_shopifyApiUrl, content);
      response.EnsureSuccessStatusCode();

      var result = await response.Content.ReadAsStringAsync();
      var json = JsonNode.Parse(result);
      var productData = json["data"]["product"];

      return new Product
      {
    Id = int.Parse(productData["id"].ToString()),
    Name = productData["title"].ToString(),
    Description = productData["descriptionHtml"].ToString(),
    Sku = productData["sku"].ToString(),
    Ean = productData["ean"].ToString(),
    Color = productData["color"].ToString(),
    Material = productData["material"].ToString(),
    ProductType = productData["productType"].ToString(),
    ProductGroup = productData["productGroup"].ToString(),
    Supplier = productData["supplier"].ToString(),
    SupplierSku = productData["supplierSku"].ToString(),
    TemplateNo = int.Parse(productData["templateNo"].ToString()),
    List = int.Parse(productData["list"].ToString()),
    Weight = float.Parse(productData["weight"].ToString()),
    Cost = float.Parse(productData["cost"].ToString()),
    Currency = productData["currency"].ToString(),
    Price = float.Parse(productData["price"].ToString()),
    SpecialPrice = float.Parse(productData["specialPrice"].ToString())
      };
  }

    // Get all products
    public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
    {
      var query = @"
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

        var product = new Product
        {
          Id = int.Parse(productNode.GetProperty("id").GetString()),
          Name = productNode.GetProperty("title").GetString(),
          Description = productNode.GetProperty("descriptionHtml").GetString(),
          Sku = productNode.GetProperty("sku").GetString(),
          Ean = productNode.GetProperty("ean").GetString(),
          Color = productNode.GetProperty("color").GetString(),
          Material = productNode.GetProperty("material").GetString(),
          ProductType = productNode.GetProperty("productType").GetString(),
          ProductGroup = productNode.GetProperty("productGroup").GetString(),
          Supplier = productNode.GetProperty("supplier").GetString(),
          SupplierSku = productNode.GetProperty("supplierSku").GetString(),
          TemplateNo = int.Parse(productNode.GetProperty("templateNo").GetString()),
          List = int.Parse(productNode.GetProperty("list").GetString()),
          Weight = float.Parse(productNode.GetProperty("weight").GetString()),
          Cost = float.Parse(productNode.GetProperty("cost").GetString()),
          Currency = productNode.GetProperty("currency").GetString(),
          Price = float.Parse(productNode.GetProperty("price").GetString()),
          SpecialPrice = float.Parse(productNode.GetProperty("specialPrice").GetString())
        };

        productList.Add(product);
      }

      return productList.AsReadOnly();
    }

    // Update a single product
    public async Task UpdateProductAsync(Product product)
  {
    var mutation = @"
      mutation {
        productUpdate(input: {
        id: """ + product.Id + @""",
        title: """ + product.Name + @""",
        descriptionHtml: """ + product.Description + @""",
        sku: """ + product.Sku + @""",
        ean: """ + product.Ean + @""",
        color: """ + product.Color + @""",
        material: """ + product.Material + @""",
        productType: """ + product.ProductType + @""",
        productGroup: """ + product.ProductGroup + @""",
        supplier: """ + product.Supplier + @""",
        supplierSku: """ + product.SupplierSku + @""",
        templateNo: " + product.TemplateNo + @",
        list: " + product.List + @",
        weight: " + product.Weight + @",
        cost: " + product.Cost + @",
        currency: """ + product.Currency + @""",
        price: " + product.Price + @",
        specialPrice: " + product.SpecialPrice + @"
        }) {
        product {
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
      }";

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
  }
}