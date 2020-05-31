using System;
using System.Threading.Tasks;
using DotnetFlix.Database.Models;
using DotnetFlix.Database.Models.ViewModels;
using DotnetFlix.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.Controllers
{
    public class ProductsController: Controller
    {
        private readonly ApiService _apiService;

        public ProductsController(ApiService apiService)
        {
            this._apiService = apiService;
        }

        public async Task<IActionResult> Index(int page = 1, string search = null)
        {
            var paginatedProducts = await _apiService.GetAsync<ProductPaginationViewModel>(ApiGateway.API_GATEWAY_PRODUCTS_PAGE + page + "/" + search);
            ViewBag.SearchString = search;

            return View(paginatedProducts);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id != Guid.Empty)
            {
                var product = await _apiService.GetAsync<Product>(ApiGateway.API_GATEWAY_PRODUCTS + id);
                if (product != null)
                {
                    return View(product);
                }
            }

            return View();
        }

    }
}