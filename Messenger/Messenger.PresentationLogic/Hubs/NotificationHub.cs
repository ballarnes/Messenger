using Messenger.App.Models;
using Messenger.BusinessLogic.Services.Interfaces;
using Messenger.PresentationLogic.Models;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.PresentationLogic.Hubs
{
    public class NotificationHub : Hub
    {
        private static List<ConnectedUser> _connectedUsers = new List<ConnectedUser>();

        private readonly IMessageService _messageService;

        public NotificationHub(
            IMessageService messageService)
        {
            _messageService = messageService;
        }

        public Task SendMessage(Message message)
        {
            _messageService.AddMessage(message.Text, message.Date, message.From, message.To);

            var receiver = _connectedUsers.FirstOrDefault(i => i.Username == message.To);

            if (receiver != null)
            {
                return Clients.Client(receiver.ConnectionId).SendAsync("Receive", message);
            }

            return Task.FromResult<string>(String.Empty);
        }

        public void ConnectUser(ConnectedUser user)
        {
            _connectedUsers.Add(user);
        }

        public void DisconnectUser(ConnectedUser user)
        {
            var connected = _connectedUsers.FirstOrDefault(i => i.Username == user.Username);

            if (connected != null)
            {
                _connectedUsers.Remove(connected);
            }
        }
    }
}