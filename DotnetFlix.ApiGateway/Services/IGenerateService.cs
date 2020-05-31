using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotnetFlix.ApiGateway.Models;

namespace DotnetFlix.ApiGateway.Services
{
    public interface IGenerateService
    {
        public Task UpdateStockInProductsApiAsync(IEnumerable<OrderItem> order);
        public Task<HttpResponseMessage> PostNewOrderToOrdersServiceApiAsync(Order order);
    }
}
