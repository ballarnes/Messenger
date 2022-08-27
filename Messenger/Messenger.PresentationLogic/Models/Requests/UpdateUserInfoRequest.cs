using System.ComponentModel.DataAnnotations;

namespace Messenger.PresentationLogic.Models.Requests
{
    public class UpdateUserInfoRequest
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;
    }
}
