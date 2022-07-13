using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.BusinessLogic.Models.Responses;
using Messenger.DataAccess.Models.Dtos;

namespace Messenger.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsers(string username);
        Task<IdResponse<int>> RegisterUser(string name, string surname, string email, string username, string password);
        Task<int?> ActivateAccount(UserDto userDto);
        Task<bool?> CheckUsername(string username);
        Task<UserDto> Login(string username, string password);
        Task<int?> DeleteUser(int id);
    }
}