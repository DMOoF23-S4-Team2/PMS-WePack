using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces;
using PMS.Application.Services;
using PMS.Core.Interfaces;
using PMS.Core.Repositories;
using PMS.Core.Repositories.Base;
using PMS.Infrastructure.Data;
using PMS.Infrastructure.Repository;
using PMS.Infrastructure.Repository.Base;
using PMS.Infrastructure.Shopify;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS to allow specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPmsWeb",
        policy =>
        {
            policy.WithOrigins("http://localhost:5002") // Add other origins as needed
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

//! Register DbContext with local DB (WePackTest)
//NOTE - Remember to run [ docker-compose  up -d ] in the root folder to start the local DB
// builder.Services.AddDbContext<PMSContext>(options =>
// {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
// });

//! Register DbContext with Shopify DB
var sqlConnection = builder.Configuration[("ConnectionStrings:Wepack:SqlDb")];
builder.Services.AddSqlServer<PMSContext>(sqlConnection, options => options.EnableRetryOnFailure()); 

// Register controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowPmsWeb");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
