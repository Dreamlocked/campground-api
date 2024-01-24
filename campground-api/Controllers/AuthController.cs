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
    public class AuthController(IConfiguration configuration, UserService userService, MessageSenderService messageSenderService) : ControllerBase
    {
        // Escribe todo authcontroller
        private readonly IConfiguration _configuration = configuration;
        private readonly UserService _userService = userService;
        private readonly MessageSenderService _messageSenderService = messageSenderService;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto userDto)
        {
            var user = await _userService.GetUserLogin(userDto);

            if(user == null) return Unauthorized();

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
                                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JwtKey") ?? _configuration["Jwt:Key"]!)),
                                        SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Evita que los scripts del lado del cliente accedan a la cookie
                Secure = true, // Asegura que la cookie sólo se envíe a través de HTTPS
                SameSite = SameSiteMode.None, // Evita que la cookie se envíe en solicitudes a otros sitios
                Expires = DateTime.UtcNow.AddDays(1) // Establece la fecha de expiración de la cookie
            };
            Response.Cookies.Append(_configuration["Jwt:CookieName"]!, jwtToken, cookieOptions);

            return Ok(new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
            });
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
                                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JwtKey") ?? _configuration["Jwt:Key"]!)),
                                            SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Evita que los scripts del lado del cliente accedan a la cookie
                    Secure = true, // Asegura que la cookie sólo se envíe a través de HTTPS
                    SameSite = SameSiteMode.None, // Evita que la cookie se envíe en solicitudes a otros sitios
                    Expires = DateTime.UtcNow.AddDays(1) // Establece la fecha de expiración de la cookie
                };
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
