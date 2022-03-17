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
        public OrderViewController(AppDbContext Db)
        {
            _db = Db;
        }
        public async Task<IActionResult> Index()
        {
             
            var items = await _db.Orders.ToListAsync();
            var list = new List<OrderViewModel>();

            
            foreach (var item in items)
            {
                var order = new OrderViewModel() { id = item.OrderId, ordertime = item.OrderPlaced };
                var orderDetails = await _db.OrderDetails.Where(s => s.OrderId == item.OrderId).ToListAsync();
                var orderItems = new List<OrderProductViewModel>();


                foreach (var orderDetail in orderDetails) {
                    var orderitem = new OrderProductViewModel() { Id = orderDetail.OrderDetailId, quantity = orderDetail.Amount};
                    var product = _db.Products.Where(s => s.Id == orderDetail.ProductId).FirstOrDefault();
                    orderitem.imageuri = product.ImagePath;
                    orderitem.ProductName = product.Description;
                    orderitem.price = product.Price;
                    orderitem.total = orderitem.price * orderitem.quantity;
                    order.price += orderitem.total;
                    orderItems.Add(orderitem);
                }
                order.products = orderItems;
                list.Add(order);
            }
            
            return View(list);
        }
    }
}
