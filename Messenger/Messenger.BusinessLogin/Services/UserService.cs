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
using Microsoft.Extensions.Logging;

namespace Messenger.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<UserDto>> GetUsers(string username)
        {
            try
            {
                var result = await _userRepository.GetUsers(username);

                if (result == null)
                {
                    return null;
                }

                var mapped = result.Select(s => _mapper.Map<UserDto>(s)).ToList();
                return mapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex);
                return null;
            }
        }

        public async Task<IdResponse<int>> RegisterUser(string name, string surname, string email, string username, string password)
        {
            try
            {
                var emailCode = new Random().Next(1000, 9999);

                var result = await _userRepository.RegisterUser(name, surname, email, username, password, false, emailCode);

                if (result == null)
                {
                    return null;
                }

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress("netmessenger@outlook.com", "Net Messenger");
                    mail.To.Add(email);
                    mail.Subject = "Registration";
                    mail.Body = $"<hr><br><h1>Your code is: {emailCode}</h1><br><hr>";
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient("smtp-mail.outlook.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("netmessenger@outlook.com", "hvYm246SCNBqqqT");
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }
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

        public async Task<bool?> CheckUsername(string username)
        {
            try
            {
                var result = await _userRepository.CheckUsername(username);

                if (result == null)
                {
                    return null;
                }

                return result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex);
                return null;
            }
        }

        public async Task<int?> ActivateAccount(UserDto userDto)
        {
            try
            {
                var result = await _userRepository.ActivateAccount(_mapper.Map<User>(userDto));

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

        public async Task<UserDto> Login(string username, string password)
        {
            try
            {
                var result = await _userRepository.Login(username, password);

                if (result == null)
                {
                    return null;
                }

                var mapped = _mapper.Map<UserDto>(result);
                return mapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex);
                return null;
            }
        }

        public async Task<int?> UpdateUserInfo(int id, string name, string surname, string email, string username)
        {
            try
            {
                var result = await _userRepository.UpdateUserInfo(id, name, surname, email, username);

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

        public async Task<int?> DeleteUser(int id)
        {
            try
            {
                var result = await _userRepository.DeleteUser(id);

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