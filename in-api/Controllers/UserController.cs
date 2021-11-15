using in_api.Services;
using in_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace in_api.Controllers
{
    [Route("api/[controller]")] // api/user
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")] // api/user/register
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Response result = await _userService.RegisterUser(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPost("login")] // api/user/login
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Response result = await _userService.LoginUser(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("getusername")] // api/user/getusername
        public IActionResult GetUserName()
        {
            return Ok(new Response {
                Success = true,
                Message = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value
            });
        }
    }
}
