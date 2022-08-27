using Messenger.App.Models;

namespace Messenger.App.Services.Interfaces
{
    public interface IHttpService
    {
        Task<User?> Login(string username, string password);
        Task<HttpResponseMessage?> ActivateAccout(string username, string emailCode);
        Task<HttpResponseMessage?> RegisterUser(string name, string surname, string email, string username, string password);
        Task<HttpResponseMessage?> CheckUsername(string username);
        Task<List<User>?> GetUsers(string username);
        Task<List<Models.Message>?> GetMessages(string firstUsername, string secondUsername);
        Task<HttpResponseMessage?> UpdateUserInfo(int id, string name, string surname, string email, string username);
    }
}
