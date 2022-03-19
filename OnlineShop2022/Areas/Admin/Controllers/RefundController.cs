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
            // gets all the active refunds that have not been confirmed or denied
            var ActiveRefundRequests = await _db.refunds.Where(s => s.IsRefunded == false && s.IsDeclined == false).ToListAsync();
            //creates a list of the refund model
            var items = new List<RefundViewModel>();
            //foreach active request
            foreach (var activeRefundRequest in ActiveRefundRequests)
            {
                //create a new refund view model with the current active requests proof image, reason and order detail being refunded
                var item = new RefundViewModel { Reason = activeRefundRequest.Reason,ProofImgPath = activeRefundRequest.ProofImgPath, OrderDetailid = activeRefundRequest.OrderdetailID};
                //retrives required information
                var Orderdetail = await _db.OrderDetails.Where(s => s.OrderDetailId == activeRefundRequest.OrderdetailID).FirstOrDefaultAsync();
                var product = _db.Products.Where(s => s.Id == Orderdetail.ProductId).FirstOrDefaultAsync();
                var order = await _db.Orders.Where(s => s.OrderId == Orderdetail.OrderId).FirstOrDefaultAsync();
                //makes the user attatched to the order the author of the review
                item.UserEmail = _userManager.FindByIdAsync(order.UserID).Result.Email;
                //sets other required information and add it to the list
                item.RefundId = activeRefundRequest.RefundId;
                item.ItemName = product.Result.Description;
                item.Orderid = Orderdetail.OrderId;
                item.Price = Orderdetail.Price;
                items.Add(item);
            }
            return View(items);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int RefundID, string Mephod)
        {
            //gets the refund being accepted or denied
            var SRefund = await _db.refunds.Where(s => s.RefundId == RefundID).FirstOrDefaultAsync();

            //save whether it is accepted of denied in the database
            if (Mephod == "Accept")
            {
                SRefund.IsRefunded = true;
            }
            if (Mephod == "Decline")
            {
                SRefund.IsDeclined = true;
            }
            _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
