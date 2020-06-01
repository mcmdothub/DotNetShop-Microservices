using DotnetFlix.OrderService.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace DotnetFlix.OrderService.Tests
{
    public class ControllerTests : IClassFixture<OrdersInitiateFixture>
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly OrdersInitiateFixture _fixture;

        public ControllerTests(ITestOutputHelper testOutput, OrdersInitiateFixture fixture)
        {
            this._testOutput = testOutput;
            this._fixture = fixture;
        }

        [Fact]
        public async Task Create_NewOrder_And_Check_ReturnOrderObject()
        {
            // Add new Order with mocking/test 
            var order = MockOrder.Order();

            // Create/Initiate Client API endpoint
            using (var client = new TestClientProvider().Client)
            {
                // Create Json Object serialized
                // https://stackoverflow.com/questions/16098573/how-to-send-json-in-c-sharp-with-datacontractjsonserializer-without-charset
                var json = JsonObjectHelper.Serialize(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send post request to API endpoint and get response
                // TRY TO CHANCE Dummy Total AND POST WILL FAIL - see method : ValidateOrder.TotalCost(Order order) / sum all order/s 
                var response = await client.PostAsync("api/orders/create", content);

                // Debug scope to see the object returned
                var newOrder = await JsonObjectHelper.Deserialize<Order>(response);

                // Test/Assert/Confirm  1
                Assert.NotNull(newOrder);                   // Check/Assert THAT returned Order is not null
                // Test/Assert/Confirm  2
                Assert.NotEqual(Guid.Empty, newOrder.Id);   // Check/Assert THAT returned Order has a valid Guid Id
                // Test/Assert/Confirm  3
                Assert.Equal(1, newOrder.StatusId);         // Check/Assert THAT returned Order has a status -Accepted- aka 1

                // Clean up and remove testorder
                using (var context = new TestDbContextProvider().DbContext)
                {
                    context.Remove(newOrder);
                    await context.SaveChangesAsync();
                }

            }
        }

        [Fact]
        public async Task GetAllOrders_ReturnOkStatus()
        {
            using (var client = new TestClientProvider().Client)
            {
                // Send request to APi endpoint
                var result = await client.GetAsync("api/orders/all");

                // Debug scope to see the object returned
                var orders = await JsonObjectHelper.Deserialize<List<Order>>(result);

                // Assert.Equal("6", orders.Count.ToString());

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            }
        }

        [Fact]
        public async Task GetExistingOrderbyId_And_ReturnOrderObject()
        {
            using (var client = new TestClientProvider().Client)
            {
                // Send request to APi endpoint with Order Id from fixture
                var response = await client.GetAsync("api/orders/" + _fixture.Order.Id);

                // Print to console what product Id was used during the test
                _testOutput.WriteLine($"Order id used for testing: {_fixture.Order.Id}");

                // Deserialize Json object to order object
                var order = await JsonObjectHelper.Deserialize<Order>(response);

                // Assert not null and matching Id's
                Assert.NotNull(order);
                Assert.Equal(_fixture.Order.Id, order.Id);
            }
        }

        
    }
}
