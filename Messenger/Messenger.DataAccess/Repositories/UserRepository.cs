using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Hospital.DataAccess.Repositories.Interfaces;
using Dapper;
using Messenger.DataAccess.Models.Entities;
using Messenger.DataAccess.Connection.Interfaces;
using System.Collections.Generic;
using Messenger.DataAccess.Infrastructure.Interfaces;

namespace Messenger.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionWrapper _connection;
        private readonly IStoredProcedureManager _storedProcedureManager;

        public UserRepository(
            IDbConnectionWrapper connection,
            IStoredProcedureManager storedProcedureManager)
        {
            _connection = connection;
            _storedProcedureManager = storedProcedureManager;
        }

        public async Task<List<User>> GetUsers(string username)
        {
            var result = await _connection.Connection
                    .QueryAsync<User>($"SELECT [Id], [Name], [Surname], [Email], [Username], [isActivated] FROM [Users] WHERE [Username] LIKE '%{username}%'");

            var users = result.ToList();

            if (users != null)
            {
                return users;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool?> CheckUsername(string username)
        {
            var result = await _connection.Connection
                    .QueryAsync<int>($"SELECT COUNT(*) FROM [Users] WHERE [Username] = '{username}'");

            if (result.FirstOrDefault() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int?> ActivateAccount(User user)
        {
            var result = await _connection.Connection
                    .QueryAsync<User>($"SELECT * FROM [Users] d WHERE [Username] = '{user.Username}' AND [isActivated] = 0");

            if (result.FirstOrDefault().EmailCode == user.EmailCode)
            {
                var userFromDb = result.FirstOrDefault();
                userFromDb.isActivated = true;

                var updateResult = await _connection.Connection.ExecuteAsync(
                "AddOrUpdateUsers",
                _storedProcedureManager.GetParameters(userFromDb),
                commandType: CommandType.StoredProcedure);

                if (result == default)
                {
                    return null;
                }

                return updateResult;
            }
            else
            {
                return null;
            }
        }

        public async Task<User> Login(string username, string password)
        {
            var result = await _connection.Connection
                .QueryAsync<User>($"SELECT [Id], [Name], [Surname], [Email], [Username], [isActivated] FROM [Users] WHERE [Username] = '{username}' AND [Password] = '{password}'");

            var user = result.FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return new User()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.Username,
                isActivated = user.isActivated
            };
        } 

        public async Task<int?> RegisterUser(string name, string surname, string email, string username, string password, bool isActivated, int emailCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@name", name, DbType.String);
            parameters.Add("@surname", surname, DbType.String);
            parameters.Add("@email", email, DbType.String);
            parameters.Add("@username", username, DbType.String);
            parameters.Add("@password", password, DbType.String);
            parameters.Add("@isActivated", isActivated, DbType.Boolean);
            parameters.Add("@emailCode", emailCode, DbType.Int32);
            parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _connection.Connection.ExecuteAsync("AddOrUpdateUsers", parameters, commandType: CommandType.StoredProcedure);

            var result = parameters.Get<int>("@id");

            if (result == default)
            {
                return null;
            }

            return result;
        }

        public async Task<int?> DeleteUser(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32);

            var result = await _connection.Connection.ExecuteAsync("DeleteUsers", parameters, commandType: CommandType.StoredProcedure);

            if (result == default)
            {
                return null;
            }

            return result;
        }
    }
}
