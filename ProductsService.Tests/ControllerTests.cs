using DotnetFlix.ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DotnetFlix.ProductService.Tests
{
    public class ControllerTests : IClassFixture<ProductsInitiateFixture>
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly ProductsInitiateFixture _productsFixture;

        public ControllerTests(ITestOutputHelper testOutput, ProductsInitiateFixture productsFixture)
        {
            this._testOutput = testOutput;
            this._productsFixture = productsFixture;
        }

        [Fact]
        public async Task GetAllProducts_ResponseOk()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/products/all");

                // Check if Response is Status 200 OK
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetExistingProductById_ReturnEqualProductId()
        {
            using (var client = new TestClientProvider().Client)
            {
                // Random Id from ProductsFixture
                var randomId = _productsFixture.Product.Id;

                // Send request to API endpoint and get response
                var response = await client.GetAsync("/api/products/" + randomId);
                var product = await JsonObjectHelper.Deserialize<Product>(response);

                // Print to console what product Id was used during the test
                _testOutput.WriteLine($"Random product id used for testing: {randomId}");

                // Assert API endpoint response product
                Assert.NotNull(product);
                Assert.Equal(randomId, product.Id);
            }
        }
    }
}
