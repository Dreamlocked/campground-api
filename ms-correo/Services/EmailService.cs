using Microsoft.AspNetCore.Mvc;
using ms_correo.Models;
using System.Net;
using System.Net.Mail;

namespace ms_correo.Services
{
    public class EmailService
    {
        public static void SendEmail(Email email)
        {
            try
            {
                Console.WriteLine(email);
/*                using(SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("sender@example.com", "password");
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new()
                    {
                        From = new MailAddress("sender@example.com")
                    };
                    foreach(string recipient in email.Recipients)
                    {
                        mailMessage.To.Add(recipient);
                    }
                    mailMessage.Subject = email.Subject;
                    mailMessage.Body = email.Body;
                    smtpClient.Send(mailMessage);
                }*/
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
