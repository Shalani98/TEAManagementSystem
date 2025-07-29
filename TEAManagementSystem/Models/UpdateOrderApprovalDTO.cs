namespace TEAManagementSystem.Models
{
    public class UpdateOrderApprovalDTO

    {
        public int OrderId { get; set; }
        public string SellerApprovalStatus { get; set; } = null!; // "Pending", "Approved", or "Rejected"
        public int SellerId { get; set; } // The seller who approves/rejects
    }
}
