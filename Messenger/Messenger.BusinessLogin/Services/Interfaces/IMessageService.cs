using Hospital.BusinessLogic.Models.Responses;
using Messenger.DataAccess.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.BusinessLogic.Services.Interfaces
{
    public interface IMessageService
    {
        Task<IdResponse<int>> AddMessage(string text, DateTime date, string from, string to);
        Task<int?> DeleteMessage(int id);
        Task<List<MessageDto>> GetMessages(string firstUsername, string secondUsername);
    }
}