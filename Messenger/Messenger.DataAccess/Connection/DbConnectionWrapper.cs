using Messenger.DataAccess.Connection.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Messenger.DataAccess.Connection
{
    public class DbConnectionWrapper : IDbConnectionWrapper
    {
        private readonly IDbConnection _connection;

        public DbConnectionWrapper(
            string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public IDbConnection Connection => _connection;
    }
}
