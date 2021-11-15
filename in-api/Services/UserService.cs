using in_api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace in_api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Response> RegisterUser(RegisterViewModel model)
        {
            if(model == null)
            {
                return new Response()
                {
                    Success = false,
                    Message = "NullError"
                };
            }
            
            IdentityUser user = new()
            {
                Email = model.Email,
                UserName = model.Username
            };

            if(await _userManager.FindByNameAsync(user.UserName) != null || model.Username == "default")
            {
                return new Response()
                {
                    Success = false,
                    Message = "UsernameAlreadyInUse"
                };
            }

            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                return new Response()
                {
                    Success = false,
                    Message = "EmailAlreadyInUse"
                };
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new Response()
                {
                    Success = false,
                    Message = "ServerError"
                };
            }

            return new Response()
            {
                Success = true,
                Message = "SuccessfullyCreated"
            };
        }

        public async Task<Response> LoginUser(LoginViewModel model)
        {
            if (model == null)
            {
                return new Response()
                {
                    Success = false,
                    Message = "NullError"
                };
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            
            if (user == null)
            {
                return new Response()
                {
                    Success = false,
                    Message = "InvalidCredentials"
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new Response()
                {
                    Success = false,
                    Message = "InvalidCredentials"
                };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new Response()
            {
                Success = true,
                Message = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo
            };
        }
    }
}
