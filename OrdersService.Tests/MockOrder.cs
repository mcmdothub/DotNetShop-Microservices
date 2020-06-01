using DotnetFlix.OrderService.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace DotnetFlix.OrderService.Tests
{
    static class MockOrder
    {
        public static Order Order()
        {
            var order = new Order()
            {
                UserId = Guid.NewGuid(),
                OrderDate = DateTime.Now,
                OrderTotal = 79.16M, // THIS Total MUST match SUM of ALL order items! see method -ValidateOrder.TotalCost(Order order)- OrderService sln
                StatusId = 1,
                OrderItems = new List<OrderItem>()
                    {
                        new OrderItem { ProductId = Guid.NewGuid(), Name="TestProduct 1", Quantity = 1, UnitPrice = 10.25M}, 
                        new OrderItem { ProductId = Guid.NewGuid(), Name="TestProduct 2", Quantity = 2, UnitPrice = 23.19M},  
                        new OrderItem { ProductId = Guid.NewGuid(), Name="TestProduct 3", Quantity = 3, UnitPrice = 7.51M},   
                    }
            };

            return order;
        }
    }
}
