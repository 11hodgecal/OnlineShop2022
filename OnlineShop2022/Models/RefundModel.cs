using System.ComponentModel.DataAnnotations;

namespace OnlineShop2022.Models
{
    //allows the storing of refunds 
    public class RefundModel
    {
        [Key]
        public int RefundId { get; set; }
        [Required]
        [MaxLength(250)]
        public string Reason { get; set; }
        public string ProofImgPath { get; set; }
        public string UserID { get; set; }
        public int OrderdetailID { get; set; }
        public bool IsRefunded { get; set; }
        public bool IsDeclined { get; set; }
        
    }
}
