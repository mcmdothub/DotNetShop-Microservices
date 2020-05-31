using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetFlix.OrderService.Data
{
    public class DbOrdersSeed
    {
        public static void Initialize(OrdersDbContext context)
        {
            context.Database.EnsureCreated();

            context.SaveChanges();
        }
    }
}
