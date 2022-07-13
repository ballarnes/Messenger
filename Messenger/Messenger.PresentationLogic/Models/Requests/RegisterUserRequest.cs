using System.ComponentModel.DataAnnotations;

namespace Messenger.PresentationLogic.Models.Requests
{
    public class RegisterUserRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Username { get; set;} = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
