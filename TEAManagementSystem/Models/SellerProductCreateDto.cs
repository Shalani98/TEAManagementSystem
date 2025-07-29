namespace TEAManagementSystem.Models
{
    public class SellerProductCreateDto
    {
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal SellingPrice { get; set; }
    }
}
