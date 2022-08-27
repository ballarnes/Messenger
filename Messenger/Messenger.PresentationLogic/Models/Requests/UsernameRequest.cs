using System.ComponentModel.DataAnnotations;

namespace Messenger.PresentationLogic.Models.Requests
{
    public class UsernameRequest
    {
        [Required]
        public string Username { get; set; } = null!;
    }
}
