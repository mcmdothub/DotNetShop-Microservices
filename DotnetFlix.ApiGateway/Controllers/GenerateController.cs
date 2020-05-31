using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.ApiGateway.Models;
using DotnetFlix.ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.ApiGateway.Controllers
{
    [Route("api/generate")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly IGenerateService _aggregate;

        public GenerateController(IGenerateService aggregate)
            => this._aggregate = aggregate;

        // Aggregate by sending async requests to OrdersService and ProducsService
        // Set output as plain text to avoid extra escapes in Json respons from OrderService
        [HttpPost("createorder")]
        [Produces("text/plain")]
        public async Task<IActionResult> CreateNewOrder(Order order)
        {
            // Post order to OrderService API endpoint
            var createOrder = await _aggregate.PostNewOrderToOrdersServiceApiAsync(order);

            // Update product quantity in stock by sending OrderItems and quantity to ProductsService API endpoint
            await _aggregate.UpdateStockInProductsApiAsync(order.OrderItems);

            // Was order successfully created?
            if (createOrder.IsSuccessStatusCode)
            {
                string orderJsonResponse = await createOrder.Content.ReadAsStringAsync();
                return Ok(orderJsonResponse);
            }
            else
                return BadRequest();

        }

    }
}
