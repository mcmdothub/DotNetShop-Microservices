using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetFlix.ApiGateway.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
