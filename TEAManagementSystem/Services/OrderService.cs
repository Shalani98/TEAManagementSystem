using System.Data.Common;
using Microsoft.Data.SqlClient;
using TEAManagementSystem.Models;
using TEAManagementSystem.Util;

namespace TEAManagementSystem.Services
{
    public class OrderService
    {
        private readonly DBConnection db = new DBConnection();

        // 1. Create Order (Customer)
        public bool CreateOrder(CreateOrderDTO dto)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = @"INSERT INTO Orders 
        (CustomerId, ProductId, SellingPrice, QuantitySold, PaymentType, 
         SellingDate, PaymentStatus, MoneyGivenDate, SellerApprovalStatus)
        VALUES 
        (@CustomerId, @ProductId, @SellingPrice, @QuantitySold, @PaymentType, 
         @SellingDate, @PaymentStatus, @MoneyGivenDate, @SellerApprovalStatus)";

            using var cmd = new SqlCommand(sql, conn);

            string paymentStatus = dto.PaymentType == "Full Payment" ? "Paid" : "Pending";
            DateTime? moneyGivenDate = dto.PaymentType == "Full Payment" ? dto.SellingDate : null;

            cmd.Parameters.AddWithValue("@CustomerId", dto.CustomerId);
            cmd.Parameters.AddWithValue("@ProductId", dto.ProductId);
            cmd.Parameters.AddWithValue("@SellingPrice", dto.SellingPrice);
            cmd.Parameters.AddWithValue("@QuantitySold", dto.QuantitySold);
            cmd.Parameters.AddWithValue("@PaymentType", dto.PaymentType);
            cmd.Parameters.AddWithValue("@SellingDate", dto.SellingDate);
            cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
            cmd.Parameters.AddWithValue("@MoneyGivenDate", (object?)moneyGivenDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SellerApprovalStatus", "Pending");

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0;
        }

        // 2. Approve or Reject Order (Seller)
        public bool ApproveOrder(UpdateOrderApprovalDTO dto, int sellerId)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = @"UPDATE Orders 
                           SET SellerId = @SellerId, SellerApprovalStatus = @Status 
                           WHERE OrderId = @OrderId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SellerId", sellerId);
            cmd.Parameters.AddWithValue("@Status", dto.SellerApprovalStatus);
            cmd.Parameters.AddWithValue("@OrderId", dto.OrderId);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0;
        }

        // 3. Update Payment Status (Seller)
        public bool UpdatePaymentStatus(UpdatePaymentStatusDTO dto)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = @"UPDATE Orders 
                           SET MoneyGivenDate = @MoneyGivenDate, PaymentStatus = @PaymentStatus 
                           WHERE OrderId = @OrderId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MoneyGivenDate", dto.MoneyGivenDate);
            cmd.Parameters.AddWithValue("@PaymentStatus", dto.PaymentStatus);
            cmd.Parameters.AddWithValue("@OrderId", dto.OrderId);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0;
        }
        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Orders";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                        SellerId = reader.IsDBNull(reader.GetOrdinal("SellerId"))
         ? 0
         : reader.GetInt32(reader.GetOrdinal("SellerId")),
                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                        QuantitySold = reader.GetInt32(reader.GetOrdinal("QuantitySold")),
                        PaymentType = reader.GetString(reader.GetOrdinal("PaymentType")),
                        MoneyGivenDate = reader.IsDBNull(reader.GetOrdinal("MoneyGivenDate"))
         ? (DateTime?)null
         : reader.GetDateTime(reader.GetOrdinal("MoneyGivenDate")),
                        PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                        SellingDate = reader.GetDateTime(reader.GetOrdinal("SellingDate")),
                        SellerApprovalStatus = reader.GetString(reader.GetOrdinal("SellerApprovalStatus"))
                    });

                }
            }

            db.ConClose();
            return orders;
        }

        // 4. Get Order By ID
        public Order? GetOrderById(int OrderId)
        {
            Order? order = null;
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Orders WHERE OrderId = @OrderId";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@OrderId", OrderId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                order = new Order
                {
                    OrderId = (int)reader["OrderId"],
                    CustomerId = (int)reader["CustomerId"],
                    ProductId = (int)reader["ProductId"],
                    SellerId = reader["SellerId"] != DBNull.Value ? (int)reader["SellerId"] : 0,
                    SellingPrice = (decimal)reader["SellingPrice"],
                    QuantitySold = (int)reader["QuantitySold"],
                    PaymentType = reader["PaymentType"].ToString()!,
                    PaymentStatus = reader["PaymentStatus"].ToString()!,
                    SellerApprovalStatus = reader["SellerApprovalStatus"].ToString()!,
                    SellingDate = (DateTime)reader["SellingDate"],
                    MoneyGivenDate = reader["MoneyGivenDate"] != DBNull.Value
                                     ? (DateTime?)reader["MoneyGivenDate"]
                                     : null
                };
            }

            db.ConClose();
            return order;
        }

    }
}
