using System.Data.Common;
using Microsoft.Data.SqlClient;

using TEAManagementSystem.Util;
using TEAManagementSystem.Models;
namespace TEAManagementSystem.Services
{
    public class CustomerService
    {
        private readonly DBConnection db = new DBConnection();


        public Customer GetByLogin(string email, string password)
        {
            Customer customer = null;
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Customer WHERE Email=@Email AND Password=@Password";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customer = new Customer
                {
                    Id = (int)reader["Id"],
                    Email = reader["Email"].ToString(),
                    Password = (string)reader["Password"]
                };
                
            }
            db.ConClose();
            return customer;
        }
        public bool Add(Customer customer)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "INSERT INTO Customer (Email, Password, Name, Address, PhoneNumber) " +
                         "VALUES (@Email, @Password, @Name, @Address, @Phone)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", customer.Email);
            cmd.Parameters.AddWithValue("@Password", customer.Password); // Hash passwords in production

            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", customer.PhoneNumber );


            int rows = cmd.ExecuteNonQuery();
            db.ConClose();
            return rows > 0;
        }
    }
}
