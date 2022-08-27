namespace Messenger.App.Models.Requests
{
    public class UpdateUserInfoRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;
    }
}
