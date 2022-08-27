using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Messenger.BusinessLogic.Models.Responses;
using Messenger.DataAccess.Repositories.Interfaces;
using Messenger.BusinessLogic.Services.Interfaces;
using Messenger.DataAccess.Models.Dtos;
using Messenger.DataAccess.Models.Entities;
using Messenger.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Messenger.BusinessLogic.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public MessageService(
            IMessageRepository messageRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<MessageDto>> GetMessages(string firstUsername, string secondUsername)
        {
            try
            {
                var result = await _messageRepository.GetMessages(firstUsername, secondUsername);

                if (result == null)
                {
                    return null;
                }

                var mapped = result.Select(s => _mapper.Map<MessageDto>(s)).ToList();
                return mapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex);
                return null;
            }
        }

        public async Task<IdResponse<int>> AddMessage(string text, DateTime date, string from, string to)
        {
            try
            {
                var result = await _messageRepository.AddMessage(text, date, from, to);

                if (result == null)
                {
                    return null;
                }

                return new IdResponse<int>()
                {
                    Id = result.Value
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex);
                return null;
            }
        }

        public async Task<int?> DeleteMessage(int id)
        {
            try
            {
                var result = await _messageRepository.DeleteMessage(id);

                if (result == null)
                {
                    return null;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex);
                return null;
            }
        }
    }
}