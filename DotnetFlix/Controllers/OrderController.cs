using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.Database.Models;
using DotnetFlix.Extensions;
using DotnetFlix.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.Controllers
{
    // Only logged in users should be able to review cart and confirm order!
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApiService _apiService;

        public OrderController(ICartService cartService, UserManager<ApplicationUser> userManager, ApiService apiService)
        {
            this._cartService = cartService;
            this._userManager = userManager;
            this._apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Get User
            var user = await _userManager.GetUserAsync(User);

            // Get items in cart
            var itemsInCart = await _cartService.GetCartItemsAsync();

            // Prepare data to be presented in View()
            ViewBag.HasItemsInCart = (itemsInCart.Count() == 0) ? false : true;
            ViewBag.HasCompleteAddress = HasShippingAddress(user);
            ViewBag.User = user;

            // Pass an empty Payment() obect to the View. Will be used by _PaymentPartial.cshtml for payment verification
            return View(new Payment());
        }

        /// <summary>
        /// Order.js : onClick "FORM : document.forms.paymentForm " -> POST 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Index([Bind]Payment model)
        {
            // Add products and quantity from cart to order
            var cart = await _cartService.GetCartItemsAsync();

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                // Create order
                var order = new Order()
                {
                    // Note: Order-Id is created by OrderService-API
                    UserId = Guid.Parse(user.Id),
                    OrderDate = DateTime.UtcNow,
                    OrderTotal = cart.Sum(x => x.Price * x.CartQuantity)
                };

                // Add items to order
                cart.ForEach(x => order.OrderItems.Add(new OrderItem
                {
                    ProductId = x.Id,
                    Name = x.Name,
                    Photo = x.Photo,
                    Quantity = x.CartQuantity,
                    UnitPrice = x.Price
                }));

                // Send order to OrderService-API via API Gateway and receive new order Id
                var newOrder = await _apiService.PostAsync(order, ApiGateway.API_GATEWAY_AGGREGATE_ORDER);

                // Request failed!
                if (newOrder == null)
                    return BadRequest();

                return Ok(newOrder.Id.ToString());
            
            }

            return BadRequest();
        }

        public async Task<IActionResult> Success(Guid orderId)
        {
            var user = await _userManager.GetUserAsync(User);

            ViewBag.CustomerEmail = user.Email;
            ViewBag.OrderId = orderId;//.Trim('"');

            // Cleart cart!
            _cartService.ClearCart();

            return View();
        }

        public async Task<IActionResult> OrderDetails(Guid id)
        {
            if (id != Guid.Empty)
            {
                var order = await _apiService.GetAsync<Order>(ApiGateway.API_GATEWAY_ORDERS + id);
                return View(order);
            }

            return View();
        }

        public async Task<IActionResult> OrderHistory()
        {
            var userId = _userManager.GetUserId(User);

            // Get orders from API
            var orders = await _apiService.GetAllAsync<Order>(ApiGateway.API_GATEWAY_ORDERS_BY_USERID + userId);
            return View(orders);
        }

        private bool HasShippingAddress(ApplicationUser user)
        {
            // Determine if user have a complete shippingaddress.
            // Use extensionmethod IsEmpty() from static class 'ExtensionMethods in Services folder.
            //if (user.FirstName.IsEmpty() ||
            //    user.LastName.IsEmpty() ||
            //    user.Address.IsEmpty() ||
            //    user.Zip.IsEmpty() ||
            //    user.City.IsEmpty() ||
            //    user.PhoneNumber.IsEmpty())
            if (user.Address.IsEmpty() )
            {
                return false;
            }

            return true;
        }
    }
}