namespace TEAManagementSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductType { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int FullQuantity { get; set; }
        public int QuantitySold { get; set; }
        public decimal Profit => SellingPrice - CostPrice;
        public int StockBalance => FullQuantity - QuantitySold;
        public DateTime DateAdded { get; set; }

        // Foreign key
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; } = null!;
    }


}
