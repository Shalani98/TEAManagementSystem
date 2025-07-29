using System.Data.Common;
using Microsoft.Data.SqlClient;

using TEAManagementSystem.Util;
using TEAManagementSystem.Models;
namespace TEAManagementSystem.Services
{
    public class RequestService
    {
        private  readonly DBConnection db=new DBConnection();
        private readonly ManufacturerService manufacturerService=new ManufacturerService();
        private readonly SellerService sellerService=new SellerService();

        public bool CreateRequest(Request request)
        {
            try
            {
                var conn = db.GetConn();
                db.ConOpen();
                string sql = @"INSERT INTO Request 
                       (SellerId, RequestMessage, ProductType, RequestDate, Status) 
                       VALUES 
                       (@SellerId, @RequestMessage, @ProductType, @RequestDate, @Status)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SellerId", request.SellerId);
                cmd.Parameters.AddWithValue("@RequestMessage", request.RequestMessage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProductType", request.ProductType);
                cmd.Parameters.AddWithValue("@RequestDate", request.RequestDate);
                cmd.Parameters.AddWithValue("@Status", request.Status);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on create request", ex.Message);
                db.ConClose();
                return false;
            }


        }
        public List<Request> GetAll()
        {
            var list = new List<Request>();
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Request";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Request
                {
                    RequestId = (int)reader["RequestId"],
                    SellerId = (int)reader["SellerId"],
                    RequestMessage = (String)reader["RequestMessage"],
                    ProductType = (string)reader["ProductType"],
                    
                    Status = reader["Status"].ToString(),
                    RequestDate = (DateTime)reader["RequestDate"]
                });
            }

            db.ConClose();
            return list;
        }
        // Get request by ID
        public Request? GetById(int requestId)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Request WHERE RequestId = @RequestId ";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@RequestId", requestId);

            using var reader = cmd.ExecuteReader();

            Request? request = null;
            if (reader.Read())
            {
                request = new Request
                {
                    RequestId = (int)reader["RequestId"],
                    SellerId = (int)reader["SellerId"],
                    RequestMessage = reader["RequestMessage"] as string,
                    ProductType = (string)reader["ProductType"],
                    Status = reader["Status"].ToString()!,
                    RequestDate = (DateTime)reader["RequestDate"]
                };
            }

            db.ConClose();
            return request;
        }
        // Update Request Status
        public bool UpdateRequestStatus(int RequestId, string Status)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "UPDATE Request SET Status = @Status WHERE RequestId = @RequestId AND Status = 'Pending'";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@RequestId", RequestId);

            int rows = cmd.ExecuteNonQuery();

            db.ConClose();
            return rows > 0;
        }
        // Approve Request
        // Approve Request
        public bool ApproveRequest(int RequestId)
        {
            var request = GetById(RequestId);
            if (request == null || request.Status != "Pending")
                return false;

            // Add your additional logic here (e.g., stock check)

            return UpdateRequestStatus(RequestId, "Approved");
        }

        // Reject Request
        public bool RejectRequest(int RequestId)
        {
            var request = GetById(RequestId);
            if (request == null || request.Status != "Pending")
                return false;

            return UpdateRequestStatus(RequestId, "Rejected");
        }
    }

}

