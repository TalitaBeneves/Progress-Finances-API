using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Progress_Finances_API;

namespace Progress_Finances_API.Controllers
{
    [Authorize]
    [Controller]
    [Route("api/[Controller]")]
    public class BuscaValorAtivoController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();

        [HttpGet("{ativo}")]
        public async Task<ActionResult> buscarValorAtivo(string ativo)
        {
            if (ativo == null)
                return BadRequest("O nome do Ativo deve ser informado");

            var token = BrapiToken.Token;
            var request = $"https://brapi.dev/api/quote/{ativo}?{token}";
            
            HttpResponseMessage res = await client.GetAsync(request);
            res.EnsureSuccessStatusCode();

            var resBody = await res.Content.ReadAsStringAsync();

            return Ok(resBody);
        }
    }
}
