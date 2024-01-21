using campground_api.Models.Dto;
using campground_api.Services;
using campground_api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // crea un claim id
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
            var jwtToken = tokenHandler.WriteToken(token);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Evita que los scripts del lado del cliente accedan a la cookie
                Secure = true, // Asegura que la cookie sólo se envíe a través de HTTPS
                SameSite = SameSiteMode.None, // Evita que la cookie se envíe en solicitudes a otros sitios
                Expires = DateTime.UtcNow.AddDays(1) // Establece la fecha de expiración de la cookie
            };
            Response.Cookies.Append(_configuration["Jwt:CookieName"], jwtToken, cookieOptions);

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

    }

}
