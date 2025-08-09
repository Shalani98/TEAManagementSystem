using System.Data.Common;
using Microsoft.Data.SqlClient;

using TEAManagementSystem.Util;
using TEAManagementSystem.Models;
namespace TEAManagementSystem.Services
{
    public class ManufacturerService
    {
        private readonly DBConnection db=new DBConnection();

        public Manufacturer GetByLogin(string email, string password)
        {
            Manufacturer manufacturer = null;
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Manufacturer WHERE Email=@Email AND Password=@Password";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                manufacturer = new Manufacturer
                {
                    Id = (int)reader["Id"],
                    Email = reader["Email"].ToString(),
                    Password = (string)reader["Password"]
                };

            }
            db.ConClose();
            return manufacturer;
        }
        public bool ManufacturerExists(int manufacturerId)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT COUNT(*) FROM Manufacturer WHERE Id = @Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", manufacturerId);

            int count = (int)cmd.ExecuteScalar();
            db.ConClose();

            return count > 0;
        }

        public bool Add(Product product)
        {
            if (!ManufacturerExists(product.ManufacturerId))
            {
                // Manufacturer does not exist, can't insert product
                return false;
            }

            var conn = db.GetConn();
            db.ConOpen();

            string sql = @"INSERT INTO Product 
        (ProductType, CostPrice, SellingPrice, FullQuantity, ManufacturerId) 
        VALUES 
        (@ProductType, @CostPrice, @SellingPrice, @FullQuantity, @ManufacturerId)";

            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ProductType", product.ProductType);
            cmd.Parameters.AddWithValue("@CostPrice", product.CostPrice);
            cmd.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
            cmd.Parameters.AddWithValue("@FullQuantity", product.FullQuantity);
            cmd.Parameters.AddWithValue("@ManufacturerId", product.ManufacturerId);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0;
        }


        public bool Delete(string productType)
        {
            var conn = db.GetConn();
            db.ConOpen();

            using var transaction = conn.BeginTransaction();

            try
            {
                // Delete from SellerProduct first
                string deleteSellerProductSql = @"
            DELETE FROM SellerProduct
            WHERE ProductId IN (SELECT ProductId FROM Product WHERE ProductType = @ProductType)";
                using var cmdSellerProduct = new SqlCommand(deleteSellerProductSql, conn, transaction);
                cmdSellerProduct.Parameters.AddWithValue("@ProductType", productType);
                cmdSellerProduct.ExecuteNonQuery();

                // Delete from Orders next
                string deleteOrdersSql = @"
            DELETE FROM Orders
            WHERE ProductId IN (SELECT ProductId FROM Product WHERE ProductType = @ProductType)";
                using var cmdOrders = new SqlCommand(deleteOrdersSql, conn, transaction);
                cmdOrders.Parameters.AddWithValue("@ProductType", productType);
                cmdOrders.ExecuteNonQuery();

                // Now delete from Product
                string deleteProductSql = "DELETE FROM Product WHERE ProductType = @ProductType";
                using var cmdProduct = new SqlCommand(deleteProductSql, conn, transaction);
                cmdProduct.Parameters.AddWithValue("@ProductType", productType);
                int rowsDeleted = cmdProduct.ExecuteNonQuery();

                transaction.Commit();

                return rowsDeleted > 0;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                db.ConClose();
            }
        }


    }
}
