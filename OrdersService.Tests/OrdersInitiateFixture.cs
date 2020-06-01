using DotnetFlix.OrderService.Models;

using System;
using System.Threading.Tasks;

namespace DotnetFlix.OrderService.Tests
{
    public sealed class OrdersInitiateFixture : IDisposable
    {

        public Order Order { get; private set; }

        public OrdersInitiateFixture()
        {
            Order = CreateMockOrder().Result;
        }

        public async Task<Order> CreateMockOrder()
        {
            using (var context = new TestDbContextProvider().DbContext)
            {
                // Create mock Order
                var order = MockOrder.Order();

                // Add/Save to Db in memory, dummy Order
                context.Orders.Add(order);
                await context.SaveChangesAsync();

                return order;
            }
        }

        public async void Dispose()
        {
            using (var context = new TestDbContextProvider().DbContext)
            {
                context.Remove(Order);
                await context.SaveChangesAsync();
            }
        }
    }
}
