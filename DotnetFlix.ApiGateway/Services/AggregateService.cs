using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotnetFlix.ApiGateway.Models;

namespace DotnetFlix.ApiGateway.Services
{
    public class AggregateService : IGenerateService
    {
        private readonly HttpClient _httpClient;
        private const string APPLICATION_DATATYPE = "application/json";
        public AggregateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Reduce and update products in stock by minimizing ordered quantity
        /// </summary>
        /// <param name="items"></param>
        public async Task UpdateStockInProductsApiAsync(IEnumerable<OrderItem> items)
        {
            // Gather Product Id and Quantity to reduce payload size
            var products = items.Select(x => new ProductsQuantity { ProductId = x.ProductId, Quantity = x.Quantity }).ToList();

            // Serialize to Json
            var json = JsonSerializer.Serialize(products);
            var payload = new StringContent(json, Encoding.UTF8, APPLICATION_DATATYPE);

            // Send payload and get a response
            var response = await _httpClient.PutAsync("https://localhost:44333/api/products/update/stock", payload);

            // If removing products from stock in ProductsService failed...
            if (!response.IsSuccessStatusCode)
            {
                // Do some loggin here...
            }
        }

        /// <summary>
        /// Post new order to OrdersServiceApi endpoint for creating new orders
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostNewOrderToOrdersServiceApiAsync(Order order)
        {
            // Serialize to Json
            var json = JsonSerializer.Serialize(order);
            var payload = new StringContent(json, Encoding.UTF8, APPLICATION_DATATYPE);

            // Send payload and get a response
            var response = await _httpClient.PostAsync("https://localhost:44380/api/orders/create", payload);

            // return status code
            return response;
        }
    }
}