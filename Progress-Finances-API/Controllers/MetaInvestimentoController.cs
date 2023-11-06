using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Progress_Finances_API.Data;
using Progress_Finances_API.Model;

namespace Progress_Finances_API.Controllers
{
    [Authorize]
    [Controller]
    [Route("api/[controller]")]
    public class MetaInvestimentoController : ControllerBase
    {
        private readonly DataContext _dc;

        public MetaInvestimentoController(DataContext context)
        {
            _dc = context;
        }

        [HttpGet("{idUsuario}")]
        public async Task<ActionResult> ListarMetaInvestimento(int idUsuario)
        {
            var listMeta = await _dc.meta.Where(id => id.IdUsuario == idUsuario).ToListAsync();

            if (listMeta == null) return BadRequest("Meta não encontrada");

            return Ok(listMeta);
        }

        [HttpPost("CadastrarOuAtualizarMetaInvestimento")]
        public async Task<ActionResult> CadastrarOuAtualizarMetaInvestimento([FromBody] MetaInvestimentos meta)
        {
            if (meta == null) return BadRequest("meta null");
            var totalPorcentagem = meta.Acoes + meta.Fiis + meta.Fixa;

            if (totalPorcentagem > 100) return BadRequest("A soma das porcentagens é maior que 100%");


            var verificaMeta = await _dc.meta.Where(id => id.IdUsuario == meta.IdUsuario).FirstOrDefaultAsync();

            if (verificaMeta == null)
            {
                _dc.meta.Add(meta);
                await _dc.SaveChangesAsync();

                return CreatedAtAction(nameof(ListarMetaInvestimento), new { id = meta.IdMeta }, meta);
            }
            else
            {
                verificaMeta.Acoes = meta.Acoes;
                verificaMeta.Fixa = meta.Fixa;
                verificaMeta.Fiis = meta.Fiis;
                verificaMeta.Nome = meta.Nome;


                _dc.meta.Update(verificaMeta);
                await _dc.SaveChangesAsync();

                return Ok(meta);
            }
        }
    }
}
