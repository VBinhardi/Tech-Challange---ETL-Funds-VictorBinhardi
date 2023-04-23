using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ConnectionFactory
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["mySqlConnectionString"].ConnectionString);
        }
    }
}
