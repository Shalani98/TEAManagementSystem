namespace TEAManagementSystem.Models
{
    public class CreateOrderDTO
    {
        public int OrderId { get; set; }
        

        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int QuantitySold { get; set; }
        public decimal SellingPrice { get; set; }
        public string PaymentType { get; set; } = null!;
        public DateTime? MoneyGivenDate { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public DateTime SellingDate { get; set; }
    }
}
