using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces;
using PMS.Application.Servises;
using PMS.Core.Repositories;
using PMS.Core.Repositories.Base;
using PMS.Infrastructure.Data;
using PMS.Infrastructure.Repository;
using PMS.Infrastructure.Repository.Base;

var builder = WebApplication.CreateBuilder(args);

// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

//! Register DbContext with local DB (WePackTest)
//NOTE - Remember to run [ docker-compose  up -d ] in the root folder to start the local DB
builder.Services.AddDbContext<PMSContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
