using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DotnetFlix.OrderService.Data;
using DotnetFlix.OrderService.Helpers;
using DotnetFlix.OrderService.Models;


namespace DotnetFlix.OrderService.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDbContext _context;

        public OrdersRepository(OrdersDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Get all orders sorted by order date in ascending order
        /// </summary>
        /// <returns>IEnumerable<Order></returns>
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.OrderByDescending(x => x.OrderDate).ToListAsync();
        }

        /// <summary>
        /// Get single order by order id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Order</returns>
        public async Task<Order> GetOrderByOrderIdAsync(Guid id)
        {
            return await _context.Orders.Where(x => x.Id == id)
                .Include(x => x.OrderItems)
                .Include(x => x.Status)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get all orders by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<Order></returns>
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid id)
        {
            return await _context.Orders.Where(x => x.UserId == id)
                .Include(x => x.OrderItems)
                .Include(x => x.Status)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Order</returns>
        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Validate order!
            if (ValidateOrder.OrderData(order))
            {
                // If ordertotal and sum of all items doesn't match, return null, order not valid!
                if (!ValidateOrder.TotalCost(order))
                    return null;

                // Set order status to 1
                order.StatusId = 1;

                // Save
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return order;
            }

            return null;
        }

        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            // Does order contain a valid Id and exist in database?
            if (order.Id != Guid.Empty && await OrderExistAsync(order.Id))
            {
                try
                {
                    // Update
                    _context.Entry(order).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (SqlException)        // using Microsoft.Data.SqlClient;
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Delete order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            // Find order
            var order = await _context.Orders.FindAsync(id);

            // No order found? Return false!
            if (order == null)
                return false;

            // Delete order from database
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Check if an order exist by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public async Task<bool> OrderExistAsync(Guid id)
            => await _context.Orders.AnyAsync(x => x.Id == id);
    }
}
