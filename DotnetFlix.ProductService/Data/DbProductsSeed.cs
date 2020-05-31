using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.ProductService.Models;

namespace DotnetFlix.ProductService.Data
{
    public static class DbProductsSeed
    {
        public static void Initialize(ProductsDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Product.Any())
                return;

            var products = new List<Product>()
            {
                new Product { Name = "Ad Astra",                Description = "Lorem ipsum dolor sit amet", Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Photo = @"https://image.tmdb.org/t/p/w1280/xBHvZcjRiWyobQ9kxBhO6B2dtRI.jpg", Price = 434.25M, Quantity = 100 },
                new Product { Name = "Bloodshot",               Description = "Lorem ipsum dolor sit amet", Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Photo = @"https://image.tmdb.org/t/p/w1280/8WUVHemHFH2ZIP6NWkwlHWsyrEL.jpg", Price = 90.46M, Quantity = 0 },
                new Product { Name = "Sonic the Hedgehog",      Description = "Lorem ipsum dolor sit amet", Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Photo = @"https://image.tmdb.org/t/p/w1280/aQvJ5WPzZgYVDrxLX4R6cLJCEaQ.jpg", Price = 77.99M, Quantity = 100 },
                new Product { Name = "Bad Boys for Life",       Description = "Lorem ipsum dolor sit amet", Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Photo = @"https://www.expob2b.es/uploads/fotos_noticias/6418-1405962887.jpg", Price = 99.95M, Quantity = 100 },
                new Product { Name = "Scoob",                   Description = "Lorem ipsum dolor sit amet", Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Photo = @"https://image.tmdb.org/t/p/w1280/jHo2M1OiH9Re33jYtUQdfzPeUkx.jpg", Price = 38.15M, Quantity = 100 },
                new Product { Name = "Joker!",                  Description = "Lorem ipsum dolor sit amet", Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Photo = @"https://image.tmdb.org/t/p/w1280/udDclJoHjfjb8Ekgsd4FDteOkCU.jpg", Price = 51.12M, Quantity = 100 },
                new Product { Name = "The Lovebirds",           Description = "Lorem ipsum dolor sit amet", Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Photo = @"https://image.tmdb.org/t/p/w1280/5jdLnvALCpK1NkeQU1z4YvOe2dZ.jpg", Price = 90.29M, Quantity = 100 },
                
                new Product { Name = "Birds of Prey",           Description = "Lorem ipsum dolor sit amet", Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Photo = @"https://image.tmdb.org/t/p/w1280/h4VB6m0RwcicVEZvzftYZyKXs6K.jpg", Price = 10.99M, Quantity = 100 },
                new Product { 
                    Name = "1917",                   
                    Description = "Lorem ipsum dolor sit amet", 
                    Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    Photo = @"https://image.tmdb.org/t/p/w1280/iZf0KyrE25z1sage4SYFLCCrMi9.jpg", 
                    Price = 87.20M, 
                    Quantity = 0 
                },
                new Product { 
                    Name = "Cars",     Description = "Lorem ipsum dolor sit amet", 
                    Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", 
                    Photo = @"https://image.tmdb.org/t/p/w1280/qa6HCwP4Z15l3hpsASz3auugEW6.jpg", 
                    Price = 285.20M, 
                    Quantity = 100 
                },
                new Product { Name = "Survive the Night ",       Description = "Lorem ipsum dolor sit amet", 
                Details = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", 
                Photo = @"https://image.tmdb.org/t/p/w1280/niyXFhGIk4W2WTcX2Eod8vx2Mfe.jpg", Price = 71.50M, Quantity = 100 },
            };

            foreach (var product in products)
                context.Add(product);

            context.SaveChanges();

        }
    }
}
