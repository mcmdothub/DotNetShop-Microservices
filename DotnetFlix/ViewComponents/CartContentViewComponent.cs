using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.ViewComponents
{
    public class CartContentViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartContentViewComponent(ICartService cartService)
        {
            this._cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get all items in cart
            var cartItems = await _cartService.GetCartItemsAsync();

            // Calculate total cost of all items
            ViewBag.CartTotal = cartItems.Sum(x => x.CartQuantity * x.Price);

            // Count items in cart
            ViewBag.CountItems = cartItems.Sum(x => x.CartQuantity);

            return View(cartItems);
        }
    }
}
