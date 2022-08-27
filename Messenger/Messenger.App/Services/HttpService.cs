using Flurl.Http;
using Messenger.App.Models;
using Messenger.App.Models.Requests;
using Messenger.App.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Messenger.App.Services
{
    public class HttpService : IHttpService
    {
        private readonly string _host = "http://localhost:5000";

        public async Task<User?> Login(string username, string password)
        {
            try
            {
                var response = await $"{_host}/api/v1/User/Login"
                    .PostJsonAsync(new { Username = username, Password = EncodingService.GetHashString(password) })
                    .ReceiveJson<User>();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex);
                return null;
            }
        }

        public async Task<HttpResponseMessage?> ActivateAccout(string username, string emailCode)
        {
            try
            {
                var httpClient = new HttpClient();

                var request = new User()
                {
                    Username = username,
                    EmailCode = Convert.ToInt32(emailCode)
                };

                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_host}/api/v1/User/ActivateAccount", content);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex);
                return null;
            }
        }

        public async Task<HttpResponseMessage?> RegisterUser(string name, string surname, string email, string username, string password)
        {
            try
            {
                var httpClient = new HttpClient();

                var request = new RegisterUserRequest()
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Username = username,
                    Password = EncodingService.GetHashString(password)
                };

                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_host}/api/v1/User/RegisterUser", content);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex);
                return null;
            }
        }

        public async Task<HttpResponseMessage?> CheckUsername(string username)
        {
            try
            {
                var httpClient = new HttpClient();

                var request = new UsernameRequest()
                {
                    Username = username
                };

                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_host}/api/v1/User/CheckUsername", content);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex);
                return null;
            }
        }

        public async Task<List<User>?> GetUsers(string username)
        {
            try
            {
                var response = await $"{_host}/api/v1/User/GetUsers"
                            .PostJsonAsync(new { Username = username })
                            .ReceiveJson<List<User>>();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex);
                return null;
            }
        }

        public async Task<List<Models.Message>?> GetMessages(string firstUsername, string secondUsername)
        {
            try
            {
                var response = await $"{_host}/api/v1/Message/GetMessages"
                    .PostJsonAsync(new { FirstUsername = firstUsername, SecondUsername = secondUsername })
                    .ReceiveJson<List<Models.Message>>();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex);
                return null;
            }
        }

        public async Task<HttpResponseMessage?> UpdateUserInfo(int id, string name, string surname, string email, string username)
        {
            try
            {
                var httpClient = new HttpClient();

                var request = new UpdateUserInfoRequest()
                {
                    Id = id,
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Username = username
                };

                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{_host}/api/v1/User/UpdateUserInfo", content);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex);
                return null;
            }
        }
    }
}
