using ChatInRealTime.Core.Dtos.Users;
using ChatInRealTime.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatInRealTime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        [HttpPost("register")]
        public async Task<IActionResult> SignUp(SignUpUserDto userDto)
        {
            var result = await _authService.RegisterUser(userDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn(SignInUserDto userDto)
        {
            var result = await _authService.LoginUser(userDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
