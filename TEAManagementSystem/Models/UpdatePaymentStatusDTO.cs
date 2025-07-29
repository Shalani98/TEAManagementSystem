namespace TEAManagementSystem.Models
{
    public class UpdatePaymentStatusDTO
    {
        public int OrderId { get; set; }
        public DateTime MoneyGivenDate { get; set; }
        public string PaymentStatus { get; set; } = null!; // "Paid" or "Pending"
    }
}
