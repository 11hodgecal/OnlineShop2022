using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop2022.Data;
using OnlineShop2022.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop2022.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager")]
    public class RefundController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<CustomUserModel> _userManager;

        public RefundController(AppDbContext db, UserManager<CustomUserModel> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var ActiveRefundRequests = await _db.refunds.Where(s => s.IsRefunded == false && s.IsDeclined == false).ToListAsync();
            var items = new List<RefundViewModel>();
            foreach (var activeRefundRequest in ActiveRefundRequests)
            {
                var item = new RefundViewModel { Reason = activeRefundRequest.Reason,ProofImgPath = activeRefundRequest.ProofImgPath, OrderDetailid = activeRefundRequest.OrderdetailID};
                var Orderdetail = await _db.OrderDetails.Where(s => s.OrderDetailId == activeRefundRequest.OrderdetailID).FirstOrDefaultAsync();
                var product = _db.Products.Where(s => s.Id == Orderdetail.ProductId).FirstOrDefaultAsync();
                var order = await _db.Orders.Where(s => s.OrderId == Orderdetail.OrderId).FirstOrDefaultAsync();
                item.UserEmail = _userManager.FindByIdAsync(order.UserID).Result.Email;
                item.ItemName = product.Result.Description;
                item.Orderid = Orderdetail.OrderId;
                item.Price = Orderdetail.Price;
                items.Add(item);
            }
            return View(items);
        }
    }
}
