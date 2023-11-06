using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Progress_Finances_API.Data;
using Progress_Finances_API.Model;
using Progress_Finances_API.Services;
using Microsoft.AspNetCore.Hosting;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Amazon.Runtime;

namespace Progress_Finances_API.Controllers
{
    [Authorize]
    [Controller]
    [Route("api/[Controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _dc;
        private readonly TokenAuth _token;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UsuarioController(DataContext context, TokenAuth token, IWebHostEnvironment hostEnvironment)
        {
            _dc = context;
            _token = token;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("cadastrarUsuario")]
        [AllowAnonymous]
        public async Task<ActionResult> CadastrarUsuario([FromBody] Usuarios user)
        {
            try
            {
                _dc.usuarios.Add(user);
                await _dc.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar usuário: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] Usuarios user)
        {
            try
            {
                var usuario = await _dc.usuarios.FirstOrDefaultAsync(u => u.Email == user.Email && u.Senha == user.Senha);
                if (usuario == null) throw new InvalidOperationException("Usuário não encontrado");

                usuario.Token = _token.GerarToken(usuario);
                await _dc.SaveChangesAsync();

                var response = new Usuarios
                {
                    IdUsuario = usuario.IdUsuario,
                    Email = usuario.Email,
                    ImagemUrl = usuario.ImagemUrl,
                    Nome = usuario.Nome,
                    Token = usuario.Token
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao fazer login: {ex.Message}");
            }
        }

        [HttpPut("EditarUsuario")]
        public async Task<ActionResult> EditarUsuario([FromBody] AtualizaUsuairo user)
        {
            try
            {
                var usuario = await _dc.usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == user.IdUsuario);
                if (usuario == null) throw new InvalidOperationException("Id não encontrado");

                if (!string.IsNullOrEmpty(user.NovaSenha))
                {
                    if (user.SenhaAtual != usuario.Senha)
                    {
                        return BadRequest("Senhas não conferem!");
                    }
                    usuario.Senha = user.NovaSenha;
                }

                usuario.Email = user.Email;
                usuario.Nome = user.Nome;

                _dc.usuarios.Update(usuario);
                await _dc.SaveChangesAsync();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar usuário: {ex.Message}");
            }
        }

        [HttpPut("RedefinirSenha")]
        [AllowAnonymous]
        public async Task<ActionResult> RedefinirSenha([FromBody] RedefinirSenha user)
        {
            try
            {
                var usuario = await _dc.usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email == user.Email);
                if (usuario == null) throw new InvalidOperationException("Id não encontrado");

                if (usuario.Email == user.Email)
                {
                    usuario.Senha = user.NovaSenha;

                    _dc.usuarios.Update(usuario);
                    await _dc.SaveChangesAsync();
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao redefinir usuário: {ex.Message}");
            }
        }


        [HttpPost("upload-image/{idUsuario}")]
        public async Task<ActionResult> AtualizarImagem(int idUsuario)
        {
            try
            {

                var usuario = await _dc.usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
                if (usuario == null) return BadRequest("Usuário não encontrado");

                var file = Request.Form.Files[0];
                if (usuario.ImagemUrl == null)
                {
                    usuario.ImagemUrl = await SaveImage(file);
                }
                else
                {
                    await DeleteImage(usuario.ImagemUrl);
                    usuario.ImagemUrl = await SaveImage(file);
                }

                var res = _dc.usuarios.Update(usuario);
                await _dc.SaveChangesAsync();

                var resposta = new Usuarios
                {
                    Email = usuario.Email,
                    ImagemUrl = usuario.ImagemUrl,
                    Nome = usuario.Nome,
                    IdUsuario = usuario.IdUsuario,
                    Token = usuario.Token,
                };

                return Ok(resposta);

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar imagem: {ex.Message}");
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                             .Take(10)
                                             .ToArray()
                                        ).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }

        [NonAction]
        public async Task DeleteImage(string imageName)
        {

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

        }

        [HttpDelete("DeletarUsuario/{id}")]
        public async Task<ActionResult> DeletarUsuario(int id)
        {
            try
            {
                var usuario = await _dc.usuarios.FindAsync(id);

                DeleteImage(usuario.ImagemUrl);

                _dc.usuarios.Remove(usuario);
                await _dc.SaveChangesAsync();

                return Ok(usuario);

            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar usuário: {ex.Message}");
            }
        }

    }
}
