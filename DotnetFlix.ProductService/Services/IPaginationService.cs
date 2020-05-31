using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.ProductService.Models;

namespace DotnetFlix.ProductService.Services
{
    public interface IPaginationService
    {
        public Task<ProductPagination> GetPaginatedProductsAsync(int page = 1, string search = null);

    }
}
