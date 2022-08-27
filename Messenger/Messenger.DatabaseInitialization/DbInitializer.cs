using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Collections;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace Messenger.DatabaseInitialization
{
    public class DbInitializer
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;
        private readonly List<DictionaryEntry> _commands;

        public DbInitializer(
            string connectionString,
            ILogger logger,
            List<DictionaryEntry> commands)
        {
            _connectionString = connectionString;
            _logger = logger;
            _commands = commands;
        }

        public void InitializeDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var command in _commands)
                {
                    ExecuteCommand(command, connection);
                }

                connection.Close();
            }
        }

        private void ExecuteCommand(DictionaryEntry entry, SqlConnection connection)
        {
            try
            {
                var server = new Server(new ServerConnection(connection));

                server.ConnectionContext.ExecuteNonQuery(entry.Value.ToString());

                _logger.LogInformation($"Executed [{entry.Key}] command.");
            }
            catch (ExecutionFailureException ex)
            {
                if (ex.HResult == -2146233087)
                {
                    _logger.LogWarning($"[{entry.Key}]: {ex.InnerException.Message}");
                }
                else
                {
                    _logger.LogError(ex.ToString(), $"An error occurred executing [{entry.Key}] command.");
                }
            }
        }
    }
}