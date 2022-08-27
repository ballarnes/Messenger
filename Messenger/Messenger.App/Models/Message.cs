namespace Messenger.App.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; } = null!;
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
    }
}
