using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InventoryManagement.DbContexts
{
    public class ApplicationDbContext : IDisposable
    {
        private readonly SqlConnection _connection;

        public ApplicationDbContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public SqlConnection Connection => _connection;

        public void Dispose()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
            _connection.Dispose();
        }
    }
}