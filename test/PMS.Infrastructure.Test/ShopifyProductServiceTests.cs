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
            var shopifyProductService = new ShopifyProductRepository(_httpClient, _mockConfiguration.Object, _mockSecretClient.Object);

            // Assert
            // Use reflection to access the private field _accessToken
            var accessTokenField = typeof(ShopifyProductRepository).GetField("_accessToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualAccessToken = accessTokenField.GetValue(shopifyProductService) as string;

            Assert.Equal("fake-secret-value", actualAccessToken);
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