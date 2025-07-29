namespace TEAManagementSystem.Models
{
    public class Request
    {
        public int RequestId { get; set; }

        // Foreign key
        public int SellerId { get; set; }
        public Seller Seller { get; set; } = null!;

        public string? RequestMessage { get; set; }
        public string ProductType { get; set; } = null!;
        public DateTime RequestDate { get; set; }

        public string Status { get; set; } = "Pending"; // 'Pending', 'Approved', 'Rejected'
    }

}
