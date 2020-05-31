using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.OrderService.Helpers;
using DotnetFlix.OrderService.Models;
using DotnetFlix.OrderService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.OrderService.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersRepository _ordersRepository;

        public OrdersController(OrdersRepository ordersRepository)
        {
            this._ordersRepository = ordersRepository;
        }

        // GET: api/orders/all
        [HttpGet("all")]
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _ordersRepository.GetAllOrdersAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrdersByOrderId(Guid id)
        {
            var order = await _ordersRepository.GetOrderByOrderIdAsync(id);

            if (order == null)
            {
                return NotFound(id);
            }

            return Ok(order);
        }

        // TODO: Create a unit test for this endpoint
        // GET: api/orders/user/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId(Guid id)
        {
            var orders = await _ordersRepository.GetOrdersByUserIdAsync(id);

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // PUT: api/orders/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutOrders(Guid id, Order order)
        {
            // validate id's and that OrderTotal matches sum of all orderitems
            if (order == null || id != order.Id || !ValidateOrder.TotalCost(order))
                return BadRequest();

            var isUpdated = await _ordersRepository.UpdateOrderAsync(order);

            if (isUpdated)
                return Ok(id);
            else
                return NoContent();
        }

        // POST: api/orders/create
        [HttpPost("create")]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            var newOrder = await _ordersRepository.CreateOrderAsync(order);

            if (newOrder == null)
            {
                return BadRequest();
            }

            return Ok(newOrder);
        }

        // DELETE: api/orders/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Order>> DeleteOrders(Guid id)
        {
            // Is id valid?
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            // Seek and destroy! ;)
            if (!await _ordersRepository.DeleteOrderAsync(id))
                return NotFound(id);

            // Order deleted!
            return Ok(id);
        }

        // Display API status...
        [HttpGet("running")]
        public ActionResult Startup()
        {
            return Ok("OrdersService running...");
        }
    }
}