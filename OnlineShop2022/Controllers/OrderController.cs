using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShop2022.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop2022.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCartModel _shoppingCart;
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly SignInManager<CustomUserModel> _SignInManager;

        public OrderController(IOrderRepository orderRepository, ShoppingCartModel shoppingCart, UserManager<CustomUserModel> userManager, SignInManager<CustomUserModel> signInManager)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _userManager = userManager;
            _SignInManager = signInManager;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        
        public IActionResult Payment(OrderModel order)
        {
            return View(order);
        }


        [HttpPost]
        public ActionResult Pay(PaymentIntentCreateRequest request)
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(request.Items),
                Currency = "gbp",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });

            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

        private int CalculateOrderAmount(Item[] items)
        {
            // Replace this constant with a calculation of the order's amount
            // Calculate the order total on the server to prevent
            // people from directly manipulating the amount on the client
            return 1400;
        }

        public class Item
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public Item[] Items { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsync(OrderModel order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, please add some products.");
            }
            // gets the current user id
            var user = _SignInManager.Context.User;
            var UserID = _userManager.GetUserAsync(user).Result.Id;
            if (ModelState.IsValid)
            {
                //passes throgh the order and the userID
                _orderRepository.CreateOrder(order,UserID);
                _shoppingCart.ClearCart();
                return RedirectToAction("Payment", order);
            }

            return View(order);
        }

        public IActionResult CheckoutComplete()
        {

            return RedirectToAction();
        }
    }
}
