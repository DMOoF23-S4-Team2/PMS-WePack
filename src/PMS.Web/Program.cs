var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("index.html");

app.UseDefaultFiles(options);
app.UseStaticFiles();

// Add a route to expose configuration dynamically
app.MapGet("/config", () => new
{
    ApiUrl = builder.Configuration["ApiUrl"]
});

app.MapFallbackToFile("index.html");

app.Run();