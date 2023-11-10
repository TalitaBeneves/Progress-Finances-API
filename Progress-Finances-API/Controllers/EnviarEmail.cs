using Microsoft.AspNetCore.Mvc;
using Progress_Finances_API.Model;
using Progress_Finances_API.Services;

namespace Progress_Finances_API.Controllers
{
    [Controller]
    [Route("api/[Controller]")]
    public class EnviarEmail : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public EnviarEmail(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("EnvioDeEmail")]
        public IActionResult EnvioDeEmail([FromBody] EnvioDeEmail email)
        {
            var templatePath = Path.Combine(_hostEnvironment.ContentRootPath, @"EmailTemplates/Notificacao/TemplateNotificacao.html");
            string htmlBody = "";

            using (StreamReader streamReader = System.IO.File.OpenText(templatePath))
            {
                htmlBody = streamReader.ReadToEnd();
            }

            var gmail = new EmailService("smtp.gmail.com", EnvioDeEmailProp.Email, EnvioDeEmailProp.Senha);
            gmail.SendEmail(emailTo: email.EmailTo, subject: email.Subject, body: htmlBody);

            return Ok("Sucesso ao enviar e-mail");
        }
    }
}
