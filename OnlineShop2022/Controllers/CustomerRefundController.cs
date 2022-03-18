using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop2022.Data;
using OnlineShop2022.Helpers;
using OnlineShop2022.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop2022.Controllers
{
    public class CustomerRefundController : Controller
    {
        private readonly AppDbContext _db;
        private IWebHostEnvironment _webHostEnvironment;
        private Images _images;
        private UserManager<CustomUserModel> _userManager;

        public CustomerRefundController(AppDbContext db, IWebHostEnvironment webHostEnvironment, Images images, UserManager<CustomUserModel> userManager)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _images = images;
            _userManager = userManager;
        }

        //allows the user to make a refund request
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Id = id;
            var refund = new RefundModel();
            refund.OrderdetailID = id;
            return View(refund);
        }
        [HttpPost]
        public async Task<IActionResult> Index(RefundModel RefundRequest)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var file = Request.Form.Files[0];

                    if (file != null)
                    {
                        //gets the uploaded image
                        var upload = Request.Form.Files[0];

                        //gets the images extension
                        string extenstion = Path.GetExtension(upload.FileName);

                        //gets the root path
                        string root = _webHostEnvironment.WebRootPath;

                        //sets the webpath
                        var webpath = $"/images/";
                        //sets the full path to the image
                        var path = $"{root}{webpath}";

                        //sets the new file name
                        var filename = $"{DateTime.Now.ToString("yymmssfff")}{extenstion}".ToLower();

                        //sets the new display image path 
                        RefundRequest.ProofImgPath = $"{path}{filename}";

                        //creates the directory set by the path
                        Directory.CreateDirectory(path);

                        //uploads the new file to the directory
                        using (var filestream = new FileStream(RefundRequest.ProofImgPath, FileMode.Create))
                        {
                            await upload.CopyToAsync(filestream);
                        }

                        //sets the new image path to the webpath followed by the filename
                        RefundRequest.ProofImgPath = $"{webpath}{filename}";
                        RefundRequest.ProofImgPath = _images.Upload(file, $"/images/products/");

                        //gets the current users id
                        RefundRequest.UserID = _userManager.FindByEmailAsync(HttpContext.User.Identity.Name).Result.Id;
                        
                        //saves the refund request
                        await _db.refunds.AddAsync(RefundRequest);
                        await _db.SaveChangesAsync();
                        
                        //redirects to the order view index
                        return RedirectToAction("Index","OrderView");
                    }
                }
                catch (Exception)
                {
                    //gets the order detail id
                    ViewBag.Id = RefundRequest.OrderdetailID;
                    //creates a new refund model
                    var refund = new RefundModel();
                    //attach the orderdetail to it and returns it to the view
                    refund.OrderdetailID = RefundRequest.OrderdetailID;
                    return View(refund);
                }

            }
            return View(RefundRequest);
        }
    }
}
