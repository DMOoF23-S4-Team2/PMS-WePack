using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PMS.Infrastructure.Data;

namespace PMS.Infrastructure.Test;

public class DatabaseConnectionTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public DatabaseConnectionTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<PMSContext>(options =>
            options.UseSqlServer("Server=localhost; Database=WePackTest; User Id=sa; Password=Team2Semester4; TrustServerCertificate=true"));

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public void CanConnectToDatabase()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<PMSContext>();
            Assert.True(context.Database.CanConnect());
        }
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}