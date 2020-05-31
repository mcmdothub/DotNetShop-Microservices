using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.ProductService.Models;
using DotnetFlix.ProductService.Repositories;

namespace DotnetFlix.ProductService.Services
{
    public class PaginationService : IPaginationService
    {

        // How many products per page should be displayed?
        private const int PRODUCTS_PER_PAGE = 6;

        // How many pagination links should be displayed?
        private const int MAX_PAGINATION_LINKS = 5;

        // Instantiate IProductsRepository from DI
        private readonly IProductsRepository _repository;

        public PaginationService(IProductsRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ProductPagination> GetPaginatedProductsAsync(int page = 1, string search = null)
        {
            // Count all products
            var totalProducts = await _repository.CountAllProductsAsync(search);

            // Calculate how many pages the products will be split into
            var totalPages = CalculateTotalPaginationPages(totalProducts, PRODUCTS_PER_PAGE);

            // If user tries to view a page past pagination-range or totalProducts is 0, return an empty view
            if (totalProducts == 0 || page > totalPages)
                return null;

            // Set default paging to page 1 if user is trying to manipulate the starting point below 1
            int currentPage = (page < 1) ? 1 : page;

            // Get products from selected page based on page number from query-string
            var products = await _repository.GetPaginatedProductsAsync(currentPage, PRODUCTS_PER_PAGE, search);

            var maxLinks = (MAX_PAGINATION_LINKS > totalPages) ? totalPages : MAX_PAGINATION_LINKS;
            var offset = CalculatePaginationOffset(totalPages, currentPage, maxLinks);

            // Instantiate a new Pagination-object
            var pagination = new ProductPagination()
            {
                PaginationData = new PaginationData()
                {
                    PaginationStart = offset,                   // Start pagination link
                    PaginationLimit = offset + (maxLinks - 1),  // Max number of pagination-links to be displayed
                    TotalPages = totalPages,                    // Total amount of pages
                    CurrentPage = currentPage,                  // Current page visitor is viewing
                    TotalProducts = totalProducts               // Total amount of products
                },

                Products = products
            };

            return pagination;
        }

        private int CalculateTotalPaginationPages(int totalProducts, int productsPerPAge)
        {
            return (int)Math.Ceiling((decimal)totalProducts / productsPerPAge);
        }

        // Calculate offset value.
        private int CalculatePaginationOffset(int totalPages, int currentPage, int maxPaginationLinks = 5)
        {
            // Start from page 1 by default
            int offset = 1;

            // Calculate position. Divide maxPaginationLinks in 2
            int position = (int)Math.Floor((decimal)maxPaginationLinks / 2);

            // If we didn't reach the end of totalPages yet, "slide" pagination 'window' and set new offset
            if (currentPage >= position && currentPage + position <= totalPages)
            {
                offset = (currentPage - position > 0) ? currentPage - position : 1;
            }
            // If we reached the end of the pagination links, don't slide pagination 'window'
            else if (currentPage >= totalPages - maxPaginationLinks && currentPage <= totalPages)
            {
                offset = (totalPages - (maxPaginationLinks - 1) > 0) ? totalPages - (maxPaginationLinks - 1) : 1;
            }

            // return new offset
            return offset;
        }
    }
}