using System;

namespace Messenger.DataAccess.Models.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
    }
}
