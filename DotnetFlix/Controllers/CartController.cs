using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.Database.Models;
using DotnetFlix.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cart;

        public CartController(ICartService cart)
        {
            this._cart = cart;
        }

        //public async Task<IActionResult> Index()
        //{
        //    // Get all items in cart
        //    var cartItems = await _cart.GetCartItemsAsync();

        //    // Calculate total cost of all items
        //    ViewBag.CartTotal = cartItems.Sum(x => x.CartQuantity * x.Price);

        //    // Count items in cart
        //    ViewBag.CountItems = cartItems.Sum(x => x.CartQuantity);

        //    // Get cart content from view component CartContentViewComponent.cs
        //    // return View("_CartContent");
        //    return View("Index",cartItems);
        //}

        public IActionResult Index()
        {
            // Get cart content from view component CartContentViewComponent.cs
            return ViewComponent("CartContent");
        }


        public async Task<ActionResult> Add(Guid id)
        {
            // Make sure no empy product id's are added to cart!
            if (id != Guid.Empty && (await _cart.AddToCartAsync(id)))
                return Ok();

            return BadRequest();
        }

        public async Task Remove(Guid id)
        {
            await _cart.RemoveOneItemAsync(id);
        }

        public void Delete(Guid id)
        {
            _cart.DeleteItemAsync(id);
        }

    }
}