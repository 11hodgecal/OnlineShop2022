namespace OnlineShop2022.Models
{
    public class OrderProductViewModel
    {
        //view model variables about a specific product ordered
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string imageuri { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public double total { get; set; }
        public bool RequestMade { get; set; }
    }
}
