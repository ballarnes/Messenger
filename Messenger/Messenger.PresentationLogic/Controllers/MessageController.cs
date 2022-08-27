using System.Net;
using System.Threading.Tasks;
using Messenger.BusinessLogic.Services.Interfaces;
using Messenger.DataAccess.Models.Dtos;
using Messenger.PresentationLogic.Models.Requests;
using Messenger.PresentationLogic.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.PresentationLogic.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(
            IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<MessageDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetMessages(GetMessagesRequest request)
        {
            var result = await _messageService.GetMessages(request.FirstUsername, request.SecondUsername);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}