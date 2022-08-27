namespace Messenger.PresentationLogic.Models.Requests
{
    public class ActivateAccountRequest
    {
        public string Username { get; set; } = null!;
        public int Code { get; set; }
    }
}
