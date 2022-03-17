using System;
using System.Collections.Generic;

namespace OnlineShop2022.Models
{
    public class OrderViewModel
    {
        // overall information about the order
        public int id { get; set; }
        public DateTime ordertime { get; set; }
        public double price { get; set; }
        public List<OrderProductViewModel> products { get; set; }
    }
}
