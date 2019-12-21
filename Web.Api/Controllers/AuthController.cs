using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web.Api.Core.Dto;
using Web.Api.Core.Models;
using Web.Api.Infrastructure.Interfaces;
using Web.Api.Serialization;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AuthSettings _authSettings;

        public AuthController(IAuthService authService, IOptions<AuthSettings> authSettings)
        {
            _authService = authService;
            _authSettings = authSettings.Value;
        }

        // Post: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginRequest loginParam)
        {
            try
            {
                LoginResponse loginResponse = await _authService.Login(loginParam);

                if (!loginResponse.Success)
                {
                    return BadRequest(new { message = loginResponse.Error.Description });
                }

                var tokenHandler = new JwtSecurityTokenHandler();

                // configure jwt authentication
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSettings.SecretKey));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.PrimarySid, loginResponse.AppUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, loginResponse.AppUser.FullName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                loginResponse.Token = tokenHandler.WriteToken(token);

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Post: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] AppUser appUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "اطلاعات وارد شده کامل نیست" });
                }

                await _authService.Register(appUser);

                return Ok("ثبت نام کاربر با موفقیت انجام یافت");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
