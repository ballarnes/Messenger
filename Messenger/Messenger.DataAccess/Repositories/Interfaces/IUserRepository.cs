using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.DataAccess.Data;
using Messenger.DataAccess.Models.Entities;

namespace Hospital.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers(string username);
        Task<bool?> CheckUsername(string username);
        Task<int?> ActivateAccount(User user);
        Task<User> Login(string username, string password);
        Task<int?> RegisterUser(string name, string surname, string email, string username, string password, bool isActivated, int emailCode);
        Task<int?> DeleteUser(int id);
    }
}
