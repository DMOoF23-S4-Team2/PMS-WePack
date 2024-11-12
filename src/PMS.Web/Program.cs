var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Configure DefaultFilesOptions to point to "index.html" explicitly
var options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("index.html");

app.UseDefaultFiles(options); // This will ensure "index.html" is served when accessing the root URL
app.UseStaticFiles();         // Allows serving of static files like CSS, JS, images, etc.
app.UseRouting();

app.MapFallbackToFile("index.html"); // Fallback for any unmatched routes

app.Run();