using DotnetFlix.ProductService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFlix.ProductService.Tests
{
    public sealed class ProductsInitiateFixture : IDisposable
    {

        public Product Product { get; private set; }
        public Product RandomProduct { get; private set; }

        public ProductsInitiateFixture()
        {
            Product = CreateMockProduct().Result;
            RandomProduct = GetRandomProductFromDatabase().Result;
        }

        public async Task<Product> CreateMockProduct()
        {
            using (var context = new TestDbContextProvider().DbContext)
            {
                // Create a new dummy test product
                var product = MockProduct.Product();

                // Add/Store this test porduct in database for unit-testing API endpoints with
                context.Product.Add(product);
                await context.SaveChangesAsync();

                return product;
            }
        }

        public async Task<Product> GetRandomProductFromDatabase()
        {
            using (var context = new TestDbContextProvider().DbContext)
            {
                // How many products are there in the database?
                var total = context.Product.Count();

                // Get a random number
                var rnd = new Random();
                var offset = rnd.Next(0, total);

                // Skip offset and take first product Id
                return await context.Product.Skip(offset).FirstOrDefaultAsync();
            }
        }

        public async void Dispose()
        {
            // Clean up and remove test product from database when all tests are done
            using (var context = new TestDbContextProvider().DbContext)
            {
                context.Remove(Product);
                await context.SaveChangesAsync();
            }
        }
    }
}
