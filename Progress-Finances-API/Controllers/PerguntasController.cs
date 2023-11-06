using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Progress_Finances_API.Data;
using Progress_Finances_API.Model;

namespace Progress_Finances_API.Controllers
{
    [Authorize]
    [Controller]
    [Route("api/[Controller]")]
    public class PerguntasController : ControllerBase
    {
        private readonly DataContext _dc;

        public PerguntasController(DataContext context)
        {
            _dc = context;

        }

        [HttpGet("buscarPerguntas/{idUsuario}")]
        public async Task<ActionResult> buscarPerguntasIdUsuario(int idUsuario)
        {

            if (idUsuario == null) return BadRequest($"IdUsuario não pode ser null");

            var perguntas = await _dc.perguntas.Where(u => u.IdUsuario == idUsuario).ToListAsync();



            return Ok(perguntas);

        }

        [HttpGet("buscarPerguntasAtivasIdUsuario/{idUsuario}")]
        public async Task<ActionResult> buscarPerguntasAtivasIdUsuario(int idUsuario)
        {
            if (idUsuario == null) return BadRequest($"IdUsuario não pode ser null");

            var perguntas = await _dc.perguntas.Where(u => u.IdUsuario == idUsuario && u.Ativo == true).ToListAsync();

            return Ok(perguntas);
        }

        [HttpPost("cadastrarPergunta")]
        public async Task<ActionResult> CadastrarPergunta([FromBody] Perguntas p)
        {
            try
            {
                if (p.Ativo == null)
                {
                    p.Ativo = true;
                };

                _dc.perguntas.Add(p);
                await _dc.SaveChangesAsync();

                return Ok(p);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar pergunta: {ex.Message}");
            }
        }

        [HttpPut("EditarPergunta")]
        public async Task<ActionResult> EditarPergunta([FromBody] Perguntas p)
        {
            try
            {
                var pergunta = await _dc.perguntas.AsNoTracking().FirstOrDefaultAsync(u => u.IdPergunta == p.IdPergunta);
                if (pergunta == null) throw new InvalidOperationException("Id não encontrado");

                pergunta.Ativo = p.Ativo;
                pergunta.Tipo = p.Tipo;
                pergunta.Pergunta = p.Pergunta;


                _dc.perguntas.Update(p);
                await _dc.SaveChangesAsync();

                return Ok(p);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar usuário: {ex.Message}");
            }
        }

        [HttpDelete("DeletarPergunta/{id}")]
        public async Task<ActionResult> DeletarPergunta(int id)
        {
            try
            {
                var pergunta = await _dc.perguntas.FindAsync(id);

                _dc.perguntas.Remove(pergunta);
                await _dc.SaveChangesAsync();

                return Ok(pergunta);

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar pergunta: {ex.Message}");
            }
        }

        [HttpPut("habilitaDesabilitaPergunta")]
        public async Task<ActionResult> habilitaDesabilitaPergunta(int IdPergunta, int IdUsuario, bool ativo)
        {
            var request = await _dc.perguntas.FirstOrDefaultAsync(id => id.IdUsuario == IdUsuario && id.IdPergunta == IdPergunta);

            if (request == null)
            {
                return BadRequest("Dados não encontrados");
            }
            else
            {
                request.Ativo = ativo;
                _dc.perguntas.Update(request);
                await _dc.SaveChangesAsync();

                return Ok();
            }
        }
    }
}
