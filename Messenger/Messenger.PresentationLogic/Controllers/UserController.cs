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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUsers(UsernameRequest request)
        {
            var result = await _userService.GetUsers(request.Username);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IdResponse<int?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            var result = await _userService.RegisterUser(request.Name, request.Surname, request.Email, request.Username, request.Password);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CheckUsername(UsernameRequest request)
        {
            var result = await _userService.CheckUsername(request.Username);

            if (result == null)
            {
                return BadRequest();
            }
            else if (!result.Value)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ActivateAccount(UserDto request)
        {
            var result = await _userService.ActivateAccount(request);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _userService.Login(request.Username, request.Password);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUserInfo(UpdateUserInfoRequest request)
        {
            var result = await _userService.UpdateUserInfo(request.Id, request.Name, request.Surname, request.Email, request.Username);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteUser(GetByIdRequest request)
        {
            var result = await _userService.DeleteUser(request.Id);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}