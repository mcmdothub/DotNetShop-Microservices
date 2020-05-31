using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.OrderService.Models;

namespace DotnetFlix.OrderService.Helpers
{
    public static class ValidateOrder
    {
        /// <summary>
        /// Validate so that totalOrder matches sum of all products
        /// </summary>
        /// <param name="order"></param>
        /// <returns>True if costs match, else false</returns>
        public static bool TotalCost(Order order)
        {
            var sumOfAllProducts = order.OrderItems.Sum(x => x.UnitPrice * x.Quantity);
            return (sumOfAllProducts != order.OrderTotal) ? false : true;
        }


        /// <summary>
        /// Validate order object and make sure properties are valid
        /// </summary>
        /// <param name="order"></param>
        /// <returns>bool</returns>
        public static bool OrderData(Order order)
        {
            // Validate product items
            foreach (var item in order.OrderItems)
            {
                if (item.ProductId == Guid.Empty ||
                    item.Quantity <= 0 ||
                    item.UnitPrice <= 0)
                    return false;
            }

            return true;
        }

    }
}
