using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DotnetFlix.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DotnetFlix.Services
{
    public class CartService : ICartService
    {
        // Get access to HttpContext object
        private IHttpContextAccessor _accessor;

        private readonly ApiService _apiService;
        private string _cartSessionCookie;

        public CartService(IConfiguration config, IHttpContextAccessor accessor, ApiService apiService)
        {
            this._accessor = accessor;
            this._apiService = apiService;
            this._cartSessionCookie = config["Cart:Name"];
        }

        /// <summary>
        /// Add product to cart by its Guid value
        /// </summary>
        /// <param name="id">Guid</param>
        public async Task<bool> AddToCartAsync(Guid id)
        {
            // Make sure no empty Guids can be added to cart!
            if (id == Guid.Empty)
                return false;

            var cartItems = new List<Cart>();

            // Does cart exist?
            if (_accessor.HttpContext.Session.GetString(_cartSessionCookie) != null)
            {
                // Get all items in cart
                cartItems = await GetCartItemsAsync();

                // Does product exist in cart?
                var productExistInCart = cartItems.Where(x => x.Id == id).FirstOrDefault();

                // Update product quantity. Add new product to cart if it doesn't exist in cart!
                // Note: productExistInCart is referencing the cartItems collection.
                if (productExistInCart != null)
                    productExistInCart.CartQuantity++;
                else
                    cartItems.Add(await AddProductAsync(id));
            }
            else
            {
                // Add new item.
                cartItems.Add(await AddProductAsync(id));
            }

            SaveCartAsync(cartItems);
            return true;
        }

        /// <summary>
        /// Get all Items in cart
        /// </summary>
        /// <returns>List of type Cart</returns>
        public async Task<List<Cart>> GetCartItemsAsync()
        {
            var cartItems = new List<Cart>();

            var json = _accessor.HttpContext.Session.GetString(_cartSessionCookie);
            if (json != null)
            {
                cartItems = JsonSerializer.Deserialize<List<Cart>>(json, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return await Task.FromResult(cartItems);
        }

        /// <summary>
        /// Decrease product quantity by Guid id
        /// </summary>
        /// <param name="id">Guid</param>
        public async Task RemoveOneItemAsync(Guid id)
        {
            var cartItems = await GetCartItemsAsync();

            // Does product exist in cart?
            var productExistInCart = cartItems.Where(x => x.Id == id).FirstOrDefault();
            productExistInCart.CartQuantity = (productExistInCart.CartQuantity - 1 <= 1) ? 1 : productExistInCart.CartQuantity - 1;

            // Save changes!
            SaveCartAsync(cartItems);
        }

        /// <summary>
        /// Delete all products from cart with Guid id
        /// </summary>
        /// <param name="id">Guid</param>
        public async Task DeleteItemAsync(Guid id)
        {
            // Get items in cart
            var cartItems = await GetCartItemsAsync();

            // Find index in collection by searching for product by its id
            var index = cartItems.IndexOf(cartItems.FirstOrDefault(x => x.Id == id));

            // Remove at index
            if (index != -1)
            {
                // Remove product(s) and save the changes
                cartItems.RemoveAt((int)index);
                SaveCartAsync(cartItems);
            }
        }

        /// <summary>
        /// Empty cart from all items
        /// </summary>
        public void ClearCart()
        {
            if (_accessor.HttpContext.Session.GetString(_cartSessionCookie) != null)
            {
                _accessor.HttpContext.Session.Remove(_cartSessionCookie);
                _accessor.HttpContext.Session.Clear();
            }
        }

        /// <summary>
        /// Add product to Cart collection
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Cart</returns>
        private async Task<Cart> AddProductAsync(Guid id)
        {
            // Get Product from API Gateway
            var product = await _apiService.GetAsync<Product>(ApiGateway.API_GATEWAY_PRODUCTS + id);

            var cartItem = new Cart
            {
                Id = product.Id,
                Name = product.Name,
                Photo = product.Photo,
                CartQuantity = 1,
                Price = product.Price
            };

            return cartItem;
        }

        /// <summary>
        /// Store JSON object in cart session cookie
        /// </summary>
        /// <param name="cart"></param>
        private void SaveCartAsync(IEnumerable<Cart> cart)
        {
            // Serialize to JSON data
            string json = JsonSerializer.Serialize(cart);

            // Store in cart session cookie
            _accessor.HttpContext.Session.SetString(_cartSessionCookie, json);
        }

    }
}
