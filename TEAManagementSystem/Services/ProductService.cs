using System.Data.Common;
using Microsoft.Data.SqlClient;

using TEAManagementSystem.Util;
using TEAManagementSystem.Models;
namespace TEAManagementSystem.Services
{
    public class ProductService
    {
        private readonly DBConnection db = new DBConnection();


        public List<Product> GetAll()
        {
            var list = new List<Product>();
            var conn = db.GetConn();
            db.ConOpen();
            string sql = "SELECT * FROM Product";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Product
                {
                    ProductId = (int)reader["ProductId"],
                    ProductType = reader["ProductType"].ToString(),
                    CostPrice = (decimal)reader["CostPrice"],
                    SellingPrice = (decimal)reader["SellingPrice"],
                    FullQuantity = (int)reader["FullQuantity"],
                    
                });






            }
            db.ConClose();
            return list;
        }
    }
}
