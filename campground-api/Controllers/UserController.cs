using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using campground_api.Models;
using campground_api.Services;
using Microsoft.AspNetCore.Authorization;
using campground_api.Models.Dto;

namespace campground_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userService.GetAll();
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            // Obtener el ID del usuario desde el token JWT
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if(string.IsNullOrEmpty(userId))
            {
                return BadRequest("No se pudo obtener el ID del usuario desde el token.");
            }

            // Obtener el usuario utilizando el ID obtenido del token
            var user = await _userService.Get(int.Parse(userId));

            if(user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, SignInDto user)
        {
            var updatedUser = await _userService.Update(id, user);
            if(updatedUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _userService.Delete(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
