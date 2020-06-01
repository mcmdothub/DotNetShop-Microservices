using DotnetFlix.ProductService.Models;
using DotnetFlix.ProductService.Repositories;
using System;
using Xunit;

namespace DotnetFlix.ProductService.Tests
{
    static class MockProduct
    {
        public static Product Product()
        {
            var product = new Product()
            {
                Name = "Test Product",
                Description = "Test Description",
                Details = "Test Details",
                Photo = @"https://image.tmdb.org/t/p/w1280/iZf0KyrE25z1sage4SYFLCCrMi9.jpg",
                Price = 87.20M,
                Quantity = 10
            };

            return product;
        }
    }
}
