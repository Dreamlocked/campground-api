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
using ms_correo.Models;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace campground_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration, UserService userService, TokenService tokenService, MessageSenderService messageSenderService) : ControllerBase
    {
        // Escribe todo authcontroller
        private readonly IConfiguration _configuration = configuration;
        private readonly UserService _userService = userService;
        private readonly MessageSenderService _messageSenderService = messageSenderService;
        private readonly TokenService _tokenService = tokenService;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto userDto)
        {
            try
            {
                var user = await _userService.GetUserLogin(userDto);

                if(user == null) return Unauthorized();

                var jwtToken = _tokenService.GenerateJwtToken(user);

                var cookieOptions = _tokenService.GenerateCookieOptions();
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
                return BadRequest("Error");
            }
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInDto newUser)
        {
            try
            {
                var user = await _userService.Create(newUser);
                await _messageSenderService.SendMessage<Email>(new Email()
                {
                    Recipients = new[] { user.Email },
                    Subject = "Bienvenido a Campground",
                    Body = $"Hola {user.FirstName} {user.LastName}, bienvenido a Campground. Ya puedes empezar a disfrutar de nuestros servicios."
                });
                return Ok(user);
            }
            catch(Exception)
            {
                return BadRequest("Ya existe un usuario registrado con ese username");
            }
        }

        [HttpPost("signout")]
        public IActionResult SignOutUser()
        {
            Response.Cookies.Delete(_configuration["Jwt:CookieName"]!);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("signin-google")]
        public async Task<IActionResult> SignInGoogle(string idToken)
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

                var jwtToken = _tokenService.GenerateJwtToken(user);

                var cookieOptions = _tokenService.GenerateCookieOptions();
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
    }
}
