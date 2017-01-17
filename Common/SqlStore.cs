using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Common
{
    public abstract class SqlStore
    {
        private readonly string _connectionString;

        protected SqlStore(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        protected async Task<IDbConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            return connection;
        }

        protected IDbConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}