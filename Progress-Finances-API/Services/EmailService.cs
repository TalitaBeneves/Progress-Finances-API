using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Progress_Finances_API.Services
{
    public class EmailService
    {
        public EmailService(string provedor, string email, string senha)
        {
            Provedor = provedor;
            Email = email;
            Senha = senha;
        }

        public string Provedor { get; private set; }
        public string Email { get; set; }
        public string Senha { get; set; }


        public void SendEmail(string emailTo, string subject, string body)
        {
            var msg = PrepareteMessage(emailTo, subject, body);

            EnviarEmailPorSMTP(msg);
        }

        private MailMessage PrepareteMessage(string emailTo, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(Email);
            if (validarEmail(emailTo))
                 mail.To.Add(emailTo);
            
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;


            return mail;

        }

        private bool validarEmail(string email)
        {
            Regex reg = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
            if (reg.IsMatch(email))
                return true;


            return false;
        }

        private void EnviarEmailPorSMTP(MailMessage message)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Host = Provedor;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Timeout = 50000;
            smtp.Credentials = new NetworkCredential(Email, Senha);
            smtp.Send(message);
            smtp.Dispose();
        }
    }


}
