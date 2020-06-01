
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DotnetFlix.OrderService.Data;

namespace DotnetFlix.OrderService.Tests
{
    public sealed class TestDbContextProvider : IDisposable
    {
        // Get acces to database context

        public OrdersDbContext DbContext { get; private set; }

        public TestDbContextProvider()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            var dbOption = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseSqlServer(config.GetConnectionString("SqlDatabase")).Options;

            var optionsBuilder = new DbContextOptionsBuilder<OrdersDbContext>();
            var context = new OrdersDbContext(dbOption);

            DbContext = context;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
