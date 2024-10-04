var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.UseDefaultFiles();

app.UseRouting();

app.Run();


// using Microsoft.AspNetCore.Components.Web;
// using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// var builder = WebAssemblyHostBuilder.CreateDefault(args);

// // Set the correct base address for the HttpClient
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7225/") });

// // Register your CategoryService as ICategoryService
// // builder.Services.AddScoped<ICategoryService, CategoryService>();

// await builder.Build().RunAsync();
