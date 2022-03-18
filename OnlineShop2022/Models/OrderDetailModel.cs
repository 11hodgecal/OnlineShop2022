using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop2022.Models
{
    //allows the order detail to be stored
    public class OrderDetailModel
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        //whether the items been refunded or not
        [DefaultValue(false)]
        public bool refunded { get; set; }
        public virtual ProductModel Product { get; set; }
        public virtual OrderModel Order { get; set; }
    }
}
