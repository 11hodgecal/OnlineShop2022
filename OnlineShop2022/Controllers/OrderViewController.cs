using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop2022.Data;
using OnlineShop2022.Models;
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

        public async Task<IActionResult> Index()
        {
            //gets the current user
            var user = _signInManager.Context.User;
            var UserID = _userManager.GetUserAsync(user).Result.Id;
            //gets the current users orders
            var items = await _db.Orders.Where(s => s.UserID == UserID).ToListAsync();

            //creates a list of order view models to be populated
            var list = new List<OrderViewModel>();

            foreach (var item in items)
            {
                //each order will create a new view model which takes the order id and order time 
                var order = new OrderViewModel() { id = item.OrderId, ordertime = item.OrderPlaced };

                //gets all the order details attached to the current order
                var orderDetails = await _db.OrderDetails.Where(s => s.OrderId == item.OrderId).ToListAsync();

                //creates a list of product view models to be populated
                var orderItems = new List<OrderProductViewModel>();
                
                foreach (var orderDetail in orderDetails) {

                    //each order detail will create a product view model which will take the order detail id and the quantity of the item
                    var orderitem = new OrderProductViewModel() { Id = orderDetail.OrderDetailId, quantity = orderDetail.Amount};

                    //gets the product attach to the detail
                    var product = _db.Products.Where(s => s.Id == orderDetail.ProductId).FirstOrDefault();

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
                list.Add(order);
            }
            //returns the order
            return View(list);
        }
    }
}
