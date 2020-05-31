using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.ProductService.Models;
using DotnetFlix.ProductService.Repositories;
using DotnetFlix.ProductService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.ProductService.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _repository;

        public ProductsController(IProductsRepository repository)
        {
            this._repository = repository;
        }

        // GET: api/products
        [HttpGet("all")]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _repository.GetAllProductsAsync();
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(id);

            return Ok(product);
        }

        // Get: api/products/page/3/optional_search_string_here
        [HttpGet("page/{pageNumber}/{search?}")]
        public async Task<ActionResult<ProductPagination>> GetPaginatedProductsFromRequestedPage(int pageNumber = 1, string search = null)
        {
            var paginationService = new PaginationService(_repository);
            var paginatedProducts = await paginationService.GetPaginatedProductsAsync(pageNumber, search);

            if (paginatedProducts == null)
                return NotFound();

            return Ok(paginatedProducts);
        }

        // POST: api/products/create
        [HttpPost("create")]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var newProduct = await _repository.AddProductAsync(product);

            if (newProduct == null)
                return BadRequest();

            return Ok(newProduct);
        }

        // PUT: api/products/update
        [HttpPut("update")]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            var result = await _repository.UpdateProductAsync(product);
            if (!result)
            {
                return NotFound(product);
            }

            return Ok(product);
        }

        // PUT: api/products/update/stock
        [HttpPut("update/stock")]
        public async Task<IActionResult> UpdateProductsInStock(IEnumerable<ProductsQuantity> products)
        {
            var result = await _repository.UpdateProductsInStockAsync(products);
            if (result)
            {
                return Ok();
            }
            else
                return BadRequest();
        }

        // Delete: api/products/{id}
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProductById(Guid id)
        {
            var result = await _repository.DeleteProductAsync(id);

            if (!result)
            {
                return NotFound(id);
            }

            return Ok(id);
        }

        [HttpGet("running")]
        public ActionResult Startup()
        {
            return Ok("ProductsService running...");
        }

    }
}
