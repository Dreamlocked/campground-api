using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using ms_correo.Models;
using System.Net;
using MailKit.Security;


namespace ms_correo.Services
{
    public class EmailService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;
        public void SendEmail(Email request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["Email:User"]));
                request.Recipients.ForEach(mail => email.To.Add(MailboxAddress.Parse(mail)));
                email.Subject = request.Subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = request.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_configuration["Email:Host"],587,SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["Email:User"], _configuration["Email:Password"]);

                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch(Exception)
            {
                Console.WriteLine("No pudo enviarse el correo");
            }
        }
    }
}
