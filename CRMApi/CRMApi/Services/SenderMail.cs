using CRMApi.Interfaces;
using MimeKit;
using System.Net.Mail;
using System.Net;
using CRMApi.Models.AccountModels;

namespace CRMApi.Services
{
    public class SenderMail : ISenderMail
    {
        private readonly IConfiguration _configuration;
        private readonly string _mail;
        private readonly string _password;
        public SenderMail(IConfiguration configuration)
        {
            _configuration = configuration;
            _mail = configuration.GetValue<string>("Mail:Login");
            _password = configuration.GetValue<string>("Mail:Password");
        }
        public void SendMail(string email, string themeMail, string message)
        {
            var smtpClient = new SmtpClient("smtp.mail.ru", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_mail, _password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_mail);
            mailMessage.To.Add(email);
            mailMessage.Subject = themeMail;
            mailMessage.Body = message;

            smtpClient.Send(mailMessage);
        }

    }
}
