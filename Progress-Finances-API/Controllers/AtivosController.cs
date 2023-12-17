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
    public class AtivosController : ControllerBase
    {
        private readonly DataContext _dc;
        private static readonly HttpClient client = new HttpClient();

        public AtivosController(DataContext context)
        {
            _dc = context;
        }

        [HttpGet("LitarAtivosById/{idUsuario}")]
        public async Task<ActionResult> LitarAtivosById(int idUsuario)
        {

            if (idUsuario == 0) return BadRequest("IdUsuario está null");

            var listAtivos = await _dc.ativos.Where(ativo => ativo.IdUsuario == idUsuario)
                                             .Include(ativo => ativo.PerguntasMarcadas)
                                             .ToListAsync();

            if (listAtivos == null) return BadRequest("Dados não encontrados.");

            var montaObj = listAtivos.Select(ativo => new Ativos
            {
                ChekedParaCalculo = ativo.ChekedParaCalculo,
                IdAtivo = ativo.IdAtivo,
                IdUsuario = ativo.IdUsuario,
                LocalAlocado = ativo.LocalAlocado,
                Nome = ativo.Nome,
                Nota = ativo.Nota,
                QtdAtivos = ativo.QtdAtivos,
                Tipo = ativo.Tipo,
                ValorTotalInvestido = ativo.ValorTotalInvestido,
                SugestaoInvestimento = ativo.SugestaoInvestimento,
                ValorAtualDoAtivo = ativo.ValorAtualDoAtivo,
                PerguntasMarcadas = ativo.PerguntasMarcadas?.Select(p => new PerguntasChecked
                {
                    AtivoId = p.AtivoId,
                    Checked = p.Checked,
                    Descricao = p.Descricao,
                    IdPergunta = p.IdPergunta,
                }).ToList()
            }).ToList();

            return Ok(montaObj);
        }


        [HttpPost("CadastrarAtivo")]
        public async Task<ActionResult> CadastrarAtivo([FromBody] Ativos ativo)
        {
            if (ativo == null) return BadRequest("Ativo está null");

            var verificaAtivo = await _dc.ativos.Where(i => i.Nome == ativo.Nome).FirstOrDefaultAsync();

            if (verificaAtivo != null)
                return BadRequest("Esse ativo já esta cadastrado");


            _dc.ativos.Add(ativo);
            await _dc.SaveChangesAsync();

            var montaObj = new Ativos
            {
                ChekedParaCalculo = ativo.ChekedParaCalculo,
                IdAtivo = ativo.IdAtivo,
                IdUsuario = ativo.IdUsuario,
                LocalAlocado = ativo.LocalAlocado,
                Nome = ativo.Nome,
                Nota = ativo.Nota,
                QtdAtivos = ativo.QtdAtivos,
                Tipo = ativo.Tipo,
                ValorTotalInvestido = ativo.ValorTotalInvestido,
                SugestaoInvestimento = ativo.SugestaoInvestimento,
                ValorAtualDoAtivo = ativo.ValorAtualDoAtivo,
                PerguntasMarcadas = ativo.PerguntasMarcadas?.Select(p => new PerguntasChecked
                {
                    AtivoId = p.AtivoId,
                    Checked = p.Checked,
                    Descricao = p.Descricao,
                    IdPergunta = p.IdPergunta,
                }).ToList()
            };

            return Created("Ativo criado com sucesso!", montaObj); ;
        }

        [HttpPut("EditarAtivo")]
        public async Task<ActionResult> EditarAtivo([FromBody] Ativos ativo)
        {
            if (ativo == null) return BadRequest("Ativo está null");

            var request = await _dc.ativos.FirstOrDefaultAsync(i => i.IdAtivo == ativo.IdAtivo);

            if (request == null)
                return BadRequest("Não foi encontrado nenhum ativo com esse id");

            request.Nome = ativo.Nome;
            request.LocalAlocado = ativo.LocalAlocado;
            request.QtdAtivos = ativo.QtdAtivos;
            request.ValorAtualDoAtivo = ativo.ValorAtualDoAtivo;

            _dc.ativos.Update(request);
            await _dc.SaveChangesAsync();

            return Ok(request);
        }

        [HttpDelete("Deletar/{idAtivo}")]
        public async Task<ActionResult> ExcluirAtivo(int idAtivo)
        {
            if (idAtivo == 0) return BadRequest("Ativo está null");

            var delet = await _dc.ativos.FirstOrDefaultAsync(i => i.IdAtivo == idAtivo);
            if (delet == null)
                return BadRequest("Não foi encontrado nenhum ativo com esse id");
            else
            {
                _dc.ativos.Remove(delet); ;
                await _dc.SaveChangesAsync();

                return Ok();
            }

        }

        //endpoint para atualizar o checked 
        //idUsuario idAtivo
        //chekedParaCalcular = ??

        [HttpPut("naoCalcularInvestimento")]
        public async Task<ActionResult> naoCalcularInvestimento(int IdAtivo, int IdUsuario, bool chekedParaCalcular)
        {
            var request = await _dc.ativos.FirstOrDefaultAsync(id => id.IdUsuario == IdUsuario && id.IdAtivo == IdAtivo);

            if (request == null)
            {
                return BadRequest("Dados não encontrados");
            }
            else
            {
                request.ChekedParaCalculo = chekedParaCalcular;
                _dc.ativos.Update(request);
                await _dc.SaveChangesAsync();

                return Ok();
            }
        }
        //verifica se usuario existe && verifica se idAtivo existe
        //existe? então atualiza


    }
}
