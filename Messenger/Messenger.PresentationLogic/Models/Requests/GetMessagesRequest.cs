namespace Messenger.PresentationLogic.Models.Requests
{
    public class GetMessagesRequest
    {
        public string FirstUsername { get; set; } = null!;
        public string SecondUsername { get; set; } = null!;
    }
}
