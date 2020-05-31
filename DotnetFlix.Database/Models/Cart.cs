using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetFlix.Database.Models
{
    public class Cart : Product
    {
        public int CartQuantity { get; set; }
    }
}