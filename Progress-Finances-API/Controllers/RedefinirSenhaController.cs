using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Progress_Finances_API.Data;
using Progress_Finances_API.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Progress_Finances_API.Controllers
{
    [Controller]
    [Route("api/[Controller]")]
    public class RedefinirSenhaController : ControllerBase
    {

        private readonly DataContext _dataContext;
        private readonly RedefinicaoDeSenha _redefinirSenha;
        public RedefinirSenhaController(DataContext dataContext, RedefinicaoDeSenha r)
        {
            _dataContext = dataContext;
            _redefinirSenha = r;
        }

        [HttpGet("RedefinirSenha")]
        public ActionResult RedefinirSenha(string email, string senha)
        {
            //chama o verificarEmail
            //var teste = new RedefinicaoDeSenha();
            _redefinirSenha.VerificaEmail(email);

            return Ok("OK");
        }

       
        /*
         2- verificar se esse email existe
         3- trata se o email não existir

        Chamar o RedefinicaoDeSenha passando os paramentros email e senha do usuario
     */
    }
}
