using DotnetFlix.ProductService.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DotnetFlix.ProductService.Tests
{
    public sealed class TestDbContextProvider : IDisposable
    {
        // Get acces to database context

        public ProductsDbContext DbContext { get; private set; }

        public TestDbContextProvider()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            var dbOption = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseSqlServer(config.GetConnectionString("SqlDatabase")).Options;

            var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
            var context = new ProductsDbContext(dbOption);

            DbContext = context;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
