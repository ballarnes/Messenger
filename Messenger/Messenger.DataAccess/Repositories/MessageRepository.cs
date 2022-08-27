using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Messenger.DataAccess.Repositories.Interfaces;
using Dapper;
using Messenger.DataAccess.Models.Entities;
using Messenger.DataAccess.Connection.Interfaces;
using System.Collections.Generic;

namespace Messenger.DataAccess.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDbConnectionWrapper _connection;

        public MessageRepository(
            IDbConnectionWrapper connection)
        {
            _connection = connection;
        }

        public async Task<List<Message>> GetMessages(string firstUsername, string secondUsername)
        {
            var result = await _connection.Connection
                    .QueryAsync<Message>($"SELECT [Id], [Text], [Date], [From], [To] FROM [Messages] WHERE ([From] = '{firstUsername}' AND [To] = '{secondUsername}') OR ([From] = '{secondUsername}' AND [To] = '{firstUsername}')");

            var messages = result.ToList();

            if (messages != null)
            {
                return messages;
            }
            else
            {
                return null;
            }
        }

        public async Task<int?> AddMessage(string text, DateTime date, string from, string to)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@date", date, DbType.Date);
            parameters.Add("@text", text, DbType.String);
            parameters.Add("@from", from, DbType.String);
            parameters.Add("@to", to, DbType.String);
            parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _connection.Connection.ExecuteAsync("AddOrUpdateMessages", parameters, commandType: CommandType.StoredProcedure);

            var result = parameters.Get<int>("@id");

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteMessage(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32);

            var result = await _connection.Connection.ExecuteAsync("DeleteMessages", parameters, commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }
    }
}
