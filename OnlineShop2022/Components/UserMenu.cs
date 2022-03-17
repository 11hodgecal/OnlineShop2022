using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace OnlineShop2022.Components
{
    public class UserMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //data for account dropdown item
            var menuItems = new List<UserMenuItem> { new UserMenuItem()
            {
                DisplayValue = "Account",
                area = "Identity",
                page = "/Account/Manage/Index"
            },
            //data for Order dropdown
            new UserMenuItem()
            {
                DisplayValue = "Orders",
                Controller = "OrderView",
                Action = "Index",
            }};

            return View(menuItems);
        }
    }
    public class UserMenuItem
    {
        public string DisplayValue { get; set; }
        public string area { get; set; }
        public string page { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
