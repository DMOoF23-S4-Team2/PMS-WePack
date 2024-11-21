using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using PMS.Application.Interfaces;
using PMS.Application.Services;
using PMS.Core.Interfaces;
using PMS.Core.Repositories;
using PMS.Core.Repositories.Base;
using PMS.Infrastructure.Data;
using PMS.Infrastructure.FileHandlers;
using PMS.Infrastructure.Interfaces;
using PMS.Infrastructure.Repository;
using PMS.Infrastructure.Repository.Base;
using PMS.Infrastructure.Shopify;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Load the ssl certificate
var certificate = new X509Certificate2("combined.pfx");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps(certificate);
    });
});

// // Add Key Vault to configuration
// if (builder.Environment.IsProduction())
//     builder.Configuration.AddAzureKeyVault(
//         new Uri("https://keyvault-wepack.vault.azure.net/"),
//         new DefaultAzureCredential(new DefaultAzureCredentialOptions
//         {
//             ManagedIdentityClientId = builder.Configuration["f19bc187-b11d-4d19-94b0-f4063acee199"]
//         }));

// // Add Azure Key Vault
// var keyVaultUri = builder.Configuration["KeyVaultUri"];
// if (!string.IsNullOrEmpty(keyVaultUri))
// {
//     var managedIdentityCredential = new DefaultAzureCredential();
//     builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), managedIdentityCredential);
// }

// Configure CORS to make API requests from your host machine and the web service in the Docker network.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPmsWeb",
        policy =>
        {
            policy.WithOrigins("http://localhost:5002", "http://pms-wepack-web:8080", "https://ca-wepack-web.bluestone-4e633029.swedencentral.azurecontainerapps.io/")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Register HttpClient
builder.Services.AddHttpClient();

// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShopifyProductRepository, ShopifyProductRepository>();

// Register Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShopifyProductService, ShopifyProductService>();
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<ICsvHandler, CsvHandler>();

// Register DbContext with Azure DB
var sqlConnection = builder.Configuration["AzureDBConnectionString"];
builder.Services.AddSqlServer<PMSContext>(sqlConnection, options => options.EnableRetryOnFailure());

// Register controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowPmsWeb");

// Add default route
app.MapGet("/", () => Results.Ok("Welcome to the API!"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();