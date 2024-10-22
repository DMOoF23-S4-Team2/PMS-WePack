using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using PMS.Core.Entities;
using PMS.Infrastructure.Shopify;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace PMS.Infrastructure.Test
{
    public class ShopifyProductServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<SecretClient> _mockSecretClient;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;

        public ShopifyProductServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockSecretClient = new Mock<SecretClient>(new Uri("https://fake-vault.vault.azure.net/"), new DefaultAzureCredential());
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _mockConfiguration.Setup(config => config["ShopifyApiUrl"]).Returns("https://dev-urrem.myshopify.com/");
            _mockConfiguration.Setup(config => config["KeyVaultUri"]).Returns("https://wepackkeys.vault.azure.net/");

            // Mock the SecretClient to return a fake secret value
            _mockSecretClient.Setup(client => client.GetSecretAsync("DevStrapAccessToken", null, default))
                .ReturnsAsync(Response.FromValue(new KeyVaultSecret("DevStrapAccessToken", "fake-secret-value"), new MockResponse(200)));
        }

        [Fact]
        public async Task Constructor_ShouldRetrieveAndSetSecrets()
        {
            // Act
            var shopifyProductService = new ShopifyProductService(_httpClient, _mockConfiguration.Object, _mockSecretClient.Object);

            // Assert
            // Use reflection to access the private field _accessToken
            var accessTokenField = typeof(ShopifyProductService).GetField("_accessToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualAccessToken = accessTokenField.GetValue(shopifyProductService) as string;

            Assert.Equal("fake-secret-value", actualAccessToken);
        }

        [Fact]
        public async Task AddProductAsync_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product
            {
            Name = "Test Product",
            Description = "Test Description",
            Sku = "12345",
            Ean = "67890",
            Color = "Red",
            Material = "Plastic",
            ProductType = "Type1",
            ProductGroup = "Group1",
            Supplier = "Supplier1",
            SupplierSku = "SupplierSku1",
            TemplateNo = 1,
            List = 2,
            Weight = 1.1f,
            Cost = 10.2f,
            Currency = "DKK",
            Price = 100.3f,
            SpecialPrice = 90.4f
            };

            var responseContent = @"
            {
            ""data"": {
                ""productCreate"": {
                ""product"": {
                    ""id"": ""1"",
                    ""title"": ""Test Product"",
                    ""descriptionHtml"": ""Test Description"",
                    ""sku"": ""12345"",
                    ""ean"": ""67890"",
                    ""color"": ""Red"",
                    ""material"": ""Plastic"",
                    ""productType"": ""Type1"",
                    ""productGroup"": ""Group1"",
                    ""supplier"": ""Supplier1"",
                    ""supplierSku"": ""SupplierSku1"",
                    ""templateNo"": 1,
                    ""list"": 2,
                    ""weight"": 1.1,
                    ""cost"": 10.2,
                    ""currency"": ""DKK"",
                    ""price"": 100.3,
                    ""specialPrice"": 90.4
                }
                }
            }
            }";

            _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });

            var shopifyProductService = new ShopifyProductService(_httpClient, _mockConfiguration.Object, _mockSecretClient.Object);

            // Act
            var result = await shopifyProductService.AddProductAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Name);
            Assert.Equal("Test Description", result.Description);
            Assert.Equal("12345", result.Sku);
            Assert.Equal("67890", result.Ean);
            Assert.Equal("Red", result.Color);
            Assert.Equal("Plastic", result.Material);
            Assert.Equal("Type1", result.ProductType);
            Assert.Equal("Group1", result.ProductGroup);
            Assert.Equal("Supplier1", result.Supplier);
            Assert.Equal("SupplierSku1", result.SupplierSku);
            Assert.Equal(1, result.TemplateNo);
            Assert.Equal(2, result.List);
            //Assert.Equal(1.1f, result.Weight);
            //Assert.Equal(10.2f, result.Cost);
            Assert.Equal("DKK", result.Currency);
            //Assert.Equal(100.3f, result.Price);
            //Assert.Equal(90.4f, result.SpecialPrice);
        }

        // MockResponse class to simulate Azure responses
        public class MockResponse : Response
        {
            private readonly int _status;
            public MockResponse(int status) => _status = status;
            public override int Status => _status;
            public override string ReasonPhrase => throw new NotImplementedException();
            public override Stream ContentStream { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override string ClientRequestId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override void Dispose() { }
            protected override bool TryGetHeader(string name, [NotNullWhen(true)] out string? value) => throw new NotImplementedException();
            protected override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values) => throw new NotImplementedException();
            protected override bool ContainsHeader(string name) => throw new NotImplementedException();
            protected override IEnumerable<HttpHeader> EnumerateHeaders() => throw new NotImplementedException();
        }
    }
}