using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace ms_correo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorreoController : ControllerBase
    {
        [HttpPost]
        public IActionResult EnviarCorreo(string body, string subject, string[] destinatarios)
        {
            try
            {
                using(SmtpClient smtpClient = new SmtpClient())
                {
                    MailMessage mailMessage = new()
                    {
                        From = new MailAddress("sender@example.com")
                    };
                    foreach(string destinatario in destinatarios)
                    {
                        mailMessage.To.Add(destinatario);
                    }
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    smtpClient.Send(mailMessage);
                }

                return Ok("Correo enviado correctamente");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}
