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
        public bool ApproveRequest(int requestId)
        {
            var request = GetById(requestId);
            if (request == null || request.Status != "Pending")
                return false;

            var conn = db.GetConn();
            db.ConOpen();

            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                // Step 1: Get Product by ProductType
                string getProductSql = "SELECT * FROM Product WHERE ProductType = @ProductType";
                Product? product = null;
                using (var getProductCmd = new SqlCommand(getProductSql, conn, transaction))
                {
                    getProductCmd.Parameters.AddWithValue("@ProductType", request.ProductType);
                    using var reader = getProductCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            ProductId = (int)reader["ProductId"],
                            ProductType = reader["ProductType"].ToString()!,
                            CostPrice = (decimal)reader["CostPrice"],
                            SellingPrice = (decimal)reader["SellingPrice"],
                            FullQuantity = (int)reader["FullQuantity"],
                            QuantitySold = (int)reader["QuantitySold"],
                            ManufacturerId = (int)reader["ManufacturerId"]
                        };
                    }
                }

                if (product == null)
                {
                    transaction.Rollback();
                    db.ConClose();
                    return false;
                }

                int availableStock = product.FullQuantity - product.QuantitySold;
                if (availableStock < 1)
                {
                    transaction.Rollback();
                    db.ConClose();
                    return false; // No stock available
                }

                // Step 2: Update Manufacturer Product QuantitySold (+1)
                string updateQtySql = "UPDATE Product SET QuantitySold = QuantitySold + 1 WHERE ProductId = @ProductId";
                using (var updateQtyCmd = new SqlCommand(updateQtySql, conn, transaction))
                {
                    updateQtyCmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                    updateQtyCmd.ExecuteNonQuery();
                }

                // Step 3: Insert into SellerProduct
                string insertSellerProductSql = @"INSERT INTO SellerProduct 
            (SellerId, ProductId, Quantity, SellingPrice, CostPrice)
            VALUES 
            (@SellerId, @ProductId, @Quantity, @SellingPrice, @CostPrice)";
                using (var insertCmd = new SqlCommand(insertSellerProductSql, conn, transaction))
                {
                    insertCmd.Parameters.AddWithValue("@SellerId", request.SellerId);
                    insertCmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                    insertCmd.Parameters.AddWithValue("@Quantity", 1); // Fixed quantity as example
                    insertCmd.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                    insertCmd.Parameters.AddWithValue("@CostPrice", product.CostPrice);
                    insertCmd.ExecuteNonQuery();
                }

                // Step 4: Update Request Status
                string updateRequestSql = "UPDATE Request SET Status = 'Approved' WHERE RequestId = @RequestId";
                using (var updateRequestCmd = new SqlCommand(updateRequestSql, conn, transaction))
                {
                    updateRequestCmd.Parameters.AddWithValue("@RequestId", request.RequestId);
                    updateRequestCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                db.ConClose();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Error during approval: " + ex.Message);
                db.ConClose();
                return false;
            }
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

