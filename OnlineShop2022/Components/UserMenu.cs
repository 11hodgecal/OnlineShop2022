using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace OnlineShop2022.Components
{
    public class UserMenu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menuItems = new List<UserMenuItem> { new UserMenuItem()
            {
                DisplayValue = "Account",
                area = "Identity",
                page = "/Account/Manage/Index"
            },
            new UserMenuItem()
            {
                DisplayValue = "Orders",
                Controller = "adfadfa",
                Action = "adfadf",
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
