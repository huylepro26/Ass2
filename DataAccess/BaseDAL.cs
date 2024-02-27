using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace DataAccess
{
    public class BaseDAL
    {
        public SalesDataProvier dataProvider { get; set; } = null;
        public SqlConnection connection = null;
        
        public BaseDAL()
        {
            var connectionString = GetConnectionString();
            dataProvider = new SalesDataProvier(connectionString);
        }
        public string GetConnectionString()
        {
            string connectionString;
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            connectionString = config["ConnectionString:MySalesDB"];
            return connectionString;
        }

        public void CloseConnection()=>dataProvider.CloseConnection(connection);
    }
}
