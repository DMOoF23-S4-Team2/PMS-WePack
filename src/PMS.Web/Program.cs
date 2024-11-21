using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Load the SSL certificate
var certificate = new X509Certificate2("combined.pfx");

// Configure Kestrel for HTTPS
if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(443, listenOptions =>
        {
            listenOptions.UseHttps(certificate);
        });
    });

    builder.Configuration["ApiUrl"] = "https://localhost:7225";
}

// Build the app (must come after configuration)
var app = builder.Build();

// Configure DefaultFilesOptions to point to "index.html" explicitly
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

// Fallback for unmatched routes
app.MapFallbackToFile("index.html");

app.Run();