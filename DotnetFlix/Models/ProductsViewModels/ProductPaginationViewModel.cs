using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.Database.Models;

namespace DotnetFlix.Models.ProductsViewModels
{
    /// <summary>
    /// NOT IN USE
    /// </summary>
    public class ProductPaginationViewModel
    {
        public ProductPaginationViewModel()
        {
            Products = new List<Product>();
        }

        public IEnumerable<Product> Products { get; set; }

        public PaginationData PaginationData { get; set; }
    }


    public class PaginationData
    {
        public int PaginationLimit { get; set; }

        public int PaginationStart { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int TotalProducts { get; set; } = 0;
    }
}
