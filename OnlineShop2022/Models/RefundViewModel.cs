namespace OnlineShop2022.Models
{
    public class RefundViewModel
    {
        //manager refund view model
        public int RefundId { get; set; }
        public int OrderDetailid { get; set; }
        public string Reason { get; set; }
        public string ProofImgPath { get; set; }
        public string UserEmail { get; set; }
        public string ItemName{ get; set; }
        public int Orderid { get; set; }
        public double Price { get; set; }
    }
}
