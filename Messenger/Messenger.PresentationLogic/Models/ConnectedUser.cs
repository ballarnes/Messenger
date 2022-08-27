namespace Messenger.PresentationLogic.Models
{
    public class ConnectedUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string ConnectionId { get; set; } = null!;
    }
}
