using Messenger.DataAccess.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.DataAccess.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<int?> AddMessage(string text, DateTime date, string from, string to);
        Task<int?> DeleteMessage(int id);
        Task<List<Message>> GetMessages(string firstUsername, string secondUsername);
    }
}