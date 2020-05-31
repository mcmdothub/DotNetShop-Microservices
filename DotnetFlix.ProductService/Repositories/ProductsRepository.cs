using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DotnetFlix.ProductService.Data;
using DotnetFlix.ProductService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotnetFlix.ProductService.Repositories
{
    public class ProductsRepository : IProductsRepository
    {   
        private readonly ProductsDbContext _context;

        public ProductsRepository(
            ProductsDbContext context
            )
        {
            this._context = context;
        }

        /// <summary>
        /// Get all products from database
        /// </summary>
        /// <returns>IEnumerable<Product></returns>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Product.ToListAsync();
        }

        /// <summary>
        /// Get single product by its Guid id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A single Product</returns>
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _context.Product.FindAsync(id);
        }

        /// <summary>
        /// Store new product in database
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Product object if successful, null if fail</returns>
        public async Task<Product> AddProductAsync(Product product)
        {
            try
            {
                _context.Product.Add(product);
                int rowsEffected = await _context.SaveChangesAsync();

                return (rowsEffected > 0) ? product : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update quantity of products in stock
        /// </summary>
        /// <param name="orderedItems"></param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateProductsInStockAsync(IEnumerable<ProductsQuantity> orderedItems)
        {
            try
            {
                // Perform update in a transaction for ensuring consistency
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in orderedItems)
                    {
                        // Get product
                        var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == item.ProductId);

                        // Make sure product quantity can't go below 0
                        if (product.Quantity - item.Quantity >= 0)
                        {
                            product.Quantity -= item.Quantity;
                            await _context.SaveChangesAsync();
                        }
                    }

                    // SQL-transaction has completed successfully
                    transaction.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Delete product from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if successful, else false</returns>
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = _context.Product.Find(id);

            if (product != null)
            {
                try
                {
                    _context.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    // _logger.LogInformation("DeleteProductAsync Exeption", ex.Message);
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Check wether a product exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public async Task<bool> ProductExistAsync(Guid id)
        {
            return await _context.Product.AnyAsync(x => x.Id == id);
        }

        /// <summary>
        /// Count total amount of products in database
        /// </summary>
        /// <param name="search"></param>
        /// <returns>An int representing total number of products in database</returns>
        public async Task<int> CountAllProductsAsync(string search = null)
        {
            if (string.IsNullOrEmpty(search))
            {
                // Count all products
                return await _context.Product.CountAsync();
            }
            else
            {
                // Count all products where search string yields a match
                search = search.ToLower();
                return await _context.Product.Where(
                        x => x.Name.ToLower().Contains(search) || x.Description.ToLower().Contains(search)
                    ).CountAsync();
            }
        }

        /// <summary>
        /// Retrieves a limited collection of paginated Products
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="productsPerPage"></param>
        /// <param name="search"></param>
        /// <returns>IEnumerable<Product></returns>
        public async Task<List<Product>> GetPaginatedProductsAsync(int currentPage, int productsPerPage, string search = null)
        {
            // If search string is empty, get products
            if (string.IsNullOrEmpty(search))
            {
                return await _context.Product.Skip((currentPage - 1) * productsPerPage)
                    .Take(productsPerPage)
                    .ToListAsync();
            }

            // Get products based on search string
            search = search.ToLower();
            return await _context.Product.Where(x => x.Name.ToLower().Contains(search) ||
                                                x.Description.ToLower().Contains(search))
                .Skip((currentPage - 1) * productsPerPage)
                .Take(productsPerPage)
                .ToListAsync();
        }

    }
}