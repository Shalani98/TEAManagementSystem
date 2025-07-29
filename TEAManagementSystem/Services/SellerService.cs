using System.Data.Common;
using Microsoft.Data.SqlClient;

using TEAManagementSystem.Util;
using TEAManagementSystem.Models;
namespace TEAManagementSystem.Services
{
    public class SellerService
    {
        private readonly DBConnection db=new DBConnection();

        public Seller GetByLogin(string email, string password)
        {
            Seller seller = null;
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Seller WHERE Email=@Email AND Password=@Password";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                seller = new Seller
                {
                    Id = (int)reader["Id"],
                    Email = reader["Email"].ToString(),
                    Password = (string)reader["Password"]
                };

            }
            db.ConClose();
            return seller;
        }

        public List<SellerProduct> GetSellerProducts(int sellerId)
        {
            List<SellerProduct> list = new List<SellerProduct>();
            var conn = db.GetConn();
            db.ConOpen();

            string sql = @"
                SELECT sp.*, p.ProductType
                FROM SellerProduct sp
                INNER JOIN Product p ON sp.ProductId = p.ProductId
                WHERE sp.SellerId = @SellerId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SellerId", sellerId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var product = new Product
                {
                    ProductId = (int)reader["ProductId"],
                    ProductType = reader["ProductType"].ToString()!
                };

                list.Add(new SellerProduct
                {
                    SellerProductId = (int)reader["SellerProductId"],
                    SellerId = (int)reader["SellerId"],
                    ProductId = (int)reader["ProductId"],
                    Product = product,
                    Quantity = (int)reader["Quantity"],
                    QuantitySold = (int)reader["QuantitySold"],
                    SellingPrice = (decimal)reader["SellingPrice"],
                    CostPrice = (decimal)reader["CostPrice"],
                    DateAdded = (DateTime)reader["DateAdded"]
                });
            }

            db.ConClose();
            return list;
        }

        public bool AddSellerProduct(SellerProduct sellerProduct)
        {
            var conn = db.GetConn();
            db.ConOpen();

            // Get cost price from Product table
            string getCostSql = "SELECT CostPrice FROM Product WHERE ProductId = @ProductId";
            using var costCmd = new SqlCommand(getCostSql, conn);
            costCmd.Parameters.AddWithValue("@ProductId", sellerProduct.ProductId);
            var costPrice = (decimal?)costCmd.ExecuteScalar();

            if (costPrice == null)
            {
                db.ConClose();
                return false;
            }

            sellerProduct.CostPrice = costPrice.Value;

            // Insert into SellerProduct
            string insertSql = @"
                INSERT INTO SellerProduct (SellerId, ProductId, Quantity, QuantitySold, SellingPrice, CostPrice, DateAdded)
                VALUES (@SellerId, @ProductId, @Quantity, @QuantitySold, @SellingPrice, @CostPrice, GETDATE())";

            using var insertCmd = new SqlCommand(insertSql, conn);
            insertCmd.Parameters.AddWithValue("@SellerId", sellerProduct.SellerId);
            insertCmd.Parameters.AddWithValue("@ProductId", sellerProduct.ProductId);
            insertCmd.Parameters.AddWithValue("@Quantity", sellerProduct.Quantity);
            insertCmd.Parameters.AddWithValue("@QuantitySold", 0);
            insertCmd.Parameters.AddWithValue("@SellingPrice", sellerProduct.SellingPrice);
            insertCmd.Parameters.AddWithValue("@CostPrice", sellerProduct.CostPrice);

            int rowsAffected = insertCmd.ExecuteNonQuery();
            db.ConClose();
            return rowsAffected > 0;
        }

    }
}
