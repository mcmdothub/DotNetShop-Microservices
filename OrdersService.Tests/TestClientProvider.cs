using DotnetFlix.OrderService.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Net.Http;

namespace DotnetFlix.OrderService.Tests
{
    sealed public class TestClientProvider : IDisposable
    {

        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            WebHostBuilder host = new WebHostBuilder();
            host.ConfigureServices(x => x.AddDbContext<OrdersDbContext>(options => options.UseSqlServer(config.GetConnectionString("SqlDatabase"))));
            host.UseStartup<Startup>();

            Server = new TestServer(host);

            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server?.Dispose();
            Client?.Dispose();
        }
    }
}
