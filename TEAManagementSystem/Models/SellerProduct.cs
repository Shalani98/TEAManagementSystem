namespace TEAManagementSystem.Models
{
    public class SellerProduct
    {
        public int SellerProductId { get; set; }
        public int SellerId { get; set; }
        public Seller Seller { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public int QuantitySold { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }

        public decimal Profit => SellingPrice - CostPrice;
        public int StockBalance => Quantity - QuantitySold;

        public DateTime DateAdded { get; set; }
    }

}
