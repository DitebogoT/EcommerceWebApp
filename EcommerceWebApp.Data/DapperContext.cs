using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceWebApp.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;
        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public System.Data.IDbConnection CreateConnection()
        {
            return new System.Data.SqlClient.SqlConnection(_connectionString);
        }
    }
}
