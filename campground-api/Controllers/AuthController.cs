using campground_api.Models;
using campground_api.Models.Dto;
using campground_api.Services;
using campground_api.Utils;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace campground_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration, UserService userService) : ControllerBase
    {
        // Escribe todo authcontroller
        private readonly IConfiguration _configuration = configuration;
        private readonly UserService _userService = userService;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Auth(LoginDto userDto)
        {
            var user = await _userService.GetUserLogin(userDto);

            if(user == null) return Unauthorized();

            var jwtToken = GenerateJwtToken(user);

            var cookieOptions = GenerateCookieOptions();
            Response.Cookies.Append(_configuration["Jwt:CookieName"]!, jwtToken, cookieOptions);

            return Ok(new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
            });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInDto newUser)
        {
            try
            {
                var user = await _userService.Create(newUser);
                return Ok(user);
            }
            catch(Exception)
            {
                return BadRequest("Ya existe un usuario registrado con ese username");
            }
        }

        [HttpPost("signout")]
        public IActionResult SignOut()
        {
            Response.Cookies.Delete(_configuration["Jwt:CookieName"]!);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("signin-google")]
        public async Task<IActionResult> Authenticate(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

                var userGoogle = new UserGoogleDto()
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    UserId = payload.Subject
                };

                var user = await _userService.GetUserGoogle(userGoogle);

                if(user is null) user = await _userService.CreateUserGoogle(userGoogle);

                var jwtToken = GenerateJwtToken(user);

                var cookieOptions = GenerateCookieOptions();
                Response.Cookies.Append(_configuration["Jwt:CookieName"]!, jwtToken, cookieOptions);

                return Ok(new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                });
            }
            catch(Exception)
            {
                return Unauthorized();
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                            new Claim("id", user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Name, user.Username),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email)
                        }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                                       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                                                          SecurityAlgorithms.HmacSha256Signature
                                                                         )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private CookieOptions GenerateCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(1)
            };
        }

    }
}
