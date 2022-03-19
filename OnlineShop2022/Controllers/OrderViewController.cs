using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop2022.Data;
using OnlineShop2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop2022.Controllers
{
    public class OrderViewController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly SignInManager<CustomUserModel> _signInManager;

        public OrderViewController(AppDbContext db, UserManager<CustomUserModel> userManager, SignInManager<CustomUserModel> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            //creates a list of order view models to be populated
            var list = new List<OrderViewModel>();

            GetUsersOrders(list);

            //returns the order
            var filter = "last 60 days";
            ViewBag.filterContent = filter;
            return View(list.OrderByDescending(s=>s.ordertime).Where(s=> s.ordertime > DateTime.Now.AddDays(-60)).ToList());

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(string filter)
        {
            //creates a list of order view models to be populated
            var list = new List<OrderViewModel>();

            GetUsersOrders(list);
            //returns the order

            ViewBag.filterContent = filter;

            if (filter == "last 60 days")
            {
                return View(list.OrderByDescending(s => s.ordertime).Where(s => s.ordertime > DateTime.Now.AddDays(-60)).ToList());
            }

            
            return View(list.OrderByDescending(s => s.ordertime).Where(s => s.ordertime > DateTime.Now.AddYears(-1)).ToList());

            
        }


        [Authorize]
        public async void GetUsersOrders(List<OrderViewModel> UserOrders)
        {
            //gets the current user
            var user = _signInManager.Context.User;
            var UserID = _userManager.GetUserAsync(user).Result.Id;
            //gets the current users orders
            var items = await _db.Orders.Where(s => s.UserID == UserID).ToListAsync();

            foreach (var item in items)
            {
                //each order will create a new view model which takes the order id and order time 
                var order = new OrderViewModel() { id = item.OrderId, ordertime = item.OrderPlaced};

                //gets all the order details attached to the current order
                var orderDetails = await _db.OrderDetails.Where(s => s.OrderId == item.OrderId).ToListAsync();

                //creates a list of product view models to be populated
                var orderItems = new List<OrderProductViewModel>();

                foreach (var orderDetail in orderDetails)
                {

                    //each order detail will create a product view model which will take the order detail id and the quantity of the item
                    var orderitem = new OrderProductViewModel() { Id = orderDetail.OrderDetailId, quantity = orderDetail.Amount };

                    //gets the product attach to the detail
                    var product = _db.Products.Where(s => s.Id == orderDetail.ProductId).FirstOrDefault();

                    //checks whether a refund requests has been made and returns a bool attached to the order item view model
                    var request = _db.refunds.Where(s => s.OrderdetailID == orderDetail.OrderId).FirstOrDefault();

                    if (request != null)
                    {
                        orderitem.RequestMade = true;
                    }
                    if (request == null)
                    {
                        orderitem.RequestMade = false;
                    }

                    //adds the relevent product information to the product view model
                    orderitem.imageuri = product.ImagePath;
                    orderitem.ProductName = product.Description;
                    orderitem.price = product.Price;
                    orderitem.total = orderitem.price * orderitem.quantity;
                    order.price += orderitem.total;
                    //adds the product view model to the order items
                    orderItems.Add(orderitem);
                }
                //attach the orderitems to the order view model
                order.products = orderItems;
                //adds the order to the order list
                UserOrders.Add(order);
            }
        }
    }
}
