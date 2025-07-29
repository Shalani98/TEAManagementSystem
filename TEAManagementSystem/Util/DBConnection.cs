using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TEAManagementSystem.Util
{
    public class DBConnection
    {
        private SqlConnection connection;

        public DBConnection()
        {
            var Constring = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"];
            connection = new SqlConnection(Constring);
        }

        public SqlConnection GetConn()
        {
            return connection;
        }

        public void ConOpen()
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
        }

        public void ConClose()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }
}