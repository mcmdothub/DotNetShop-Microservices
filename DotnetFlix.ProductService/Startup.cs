using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.ProductService.Data;
using DotnetFlix.ProductService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotnetFlix.ProductService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Get connectionstring from appsettings.json
            services.AddDbContext<ProductsDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlDatabase"));
            });

            services.AddTransient<IProductsRepository, ProductsRepository>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ProductsDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Activate SSL
            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            // Seed products to database on startup
            DbProductsSeed.Initialize(context);
        }
    }
}
