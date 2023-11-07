using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Progress_Finances_API.Model;
using Progress_Finances_API.Services;
namespace Progress_Finances_API.Controllers
{
    [Controller]
    [Route("api/[Controller]")]
    public class EnviarEmail : ControllerBase
    {        
        [HttpPost("EnvioDeEmail")]
        public IActionResult EnvioDeEmail([FromBody] EnvioDeEmail email)
        {
            var gmail = new EmailService("smtp.gmail.com", EnvioDeEmailProp.Email, EnvioDeEmailProp.Senha);
            gmail.SendEmail(emailTo: email.EmailTo, subject: email.Subject, body: email.Body);

            return Ok("Sucesso ao enviar e-mail");
        }
    }
}
