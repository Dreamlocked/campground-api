using Campground.Services.Emails.Models;
using Campground.Services.Emails.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Campground.Services.Emails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(EmailService emailService) : ControllerBase
    {
        private readonly EmailService _emailService = emailService;

        // Crea metodo para enviar correo
        [HttpPost]
        public IActionResult Send(Email email)
        {
            _emailService.SendEmail(email);
            return Ok();
        }


    }
}
