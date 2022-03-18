using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop2022.Models
{
    public class OrderDetailModel
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        [DefaultValue(false)]
        public bool refunded { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual OrderModel Order { get; set; }
    }
}
