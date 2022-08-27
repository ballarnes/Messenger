using System.ComponentModel.DataAnnotations;

namespace Messenger.PresentationLogic.Models.Requests
{
    public class GetByIdRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
