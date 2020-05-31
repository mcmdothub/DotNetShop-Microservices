using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.OrderService.Models;

namespace DotnetFlix.OrderService.Repositories
{
    public interface IOrdersRepository
    {
        public Task<IEnumerable<Order>> GetAllOrdersAsync();
        public Task<Order> GetOrderByOrderIdAsync(Guid id);
        public Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid id);
        public Task<Order> CreateOrderAsync(Order order);
        public Task<bool> UpdateOrderAsync(Order order);
        public Task<bool> DeleteOrderAsync(Guid id);
        public Task<bool> OrderExistAsync(Guid id);
    }
}
