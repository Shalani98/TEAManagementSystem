using System.ComponentModel.DataAnnotations.Schema;

namespace TEAManagementSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public Seller Seller { get; set; } = null!;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int QuantitySold { get; set; }
        public decimal SellingPrice { get; set; }

        public string PaymentType { get; set; } = null!;
        public DateTime? MoneyGivenDate { get; set; }

        public string PaymentStatus { get; set; } = null!;
        public DateTime SellingDate { get; set; }

        public string SellerApprovalStatus { get; set; } = "Pending";
    }
}
