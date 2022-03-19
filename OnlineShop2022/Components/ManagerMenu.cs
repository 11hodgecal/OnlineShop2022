using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop2022.Components
{
    public class ManagerMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<ManagerMenuItem> { new ManagerMenuItem()
            {
                DisplayValue = "Order Management",
                ControllerValue = "Order"
            },
            new ManagerMenuItem()
            {
                DisplayValue = "Product Management",
                ControllerValue = "Product"
            },
            new ManagerMenuItem()
            {
                DisplayValue = "Manage Refunds",
                ControllerValue = "Refund"
            }};

            return View(menuItems);
        }
    }

    public class ManagerMenuItem
    {
        public string DisplayValue { get; set; }
        public string ControllerValue { get; set; }
    }
}
