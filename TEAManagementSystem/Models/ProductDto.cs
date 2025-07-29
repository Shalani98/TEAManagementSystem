namespace TEAManagementSystem.Models
{
    public class ProductDto
    {
        public string ProductType { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int FullQuantity { get; set; }
        public int ManufacturerId { get; set; }  // Only this is needed!
    }

}
