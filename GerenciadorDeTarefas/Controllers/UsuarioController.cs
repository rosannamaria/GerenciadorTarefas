using GerenciadorDeTarefas.Dtos;
using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController (ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SalvarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                var erros = new List<string>();
                if (string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Nome) || usuario.Nome.Length < 2)
                   
                {
                    erros.Add("Nome Inválido");
                }

                if (string.IsNullOrEmpty(usuario.Senha) || string.IsNullOrWhiteSpace(usuario.Senha) || usuario.Senha.Length < 4 &&
                    Regex.IsMatch(usuario.Senha, "[a-zA-Z0-9]+", RegexOptions.IgnoreCase))
                {
                    erros.Add("Senha Inválida");
                }
                Regex regex = new Regex(@"^([\w\.\-\+\D]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (string.IsNullOrWhiteSpace(usuario.Email) || string.IsNullOrEmpty(usuario.Email)
                    || !regex.Match(usuario.Email).Success)

                {
                    erros.Add("E-mail inválido");
                }

                if (erros.Count > 0)
                {
                    return BadRequest(new ErroRespostaDto()

                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erros = erros
                    
                        });
            }
            

                return Ok(usuario);

            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu erro ao Salvar Usuário", e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErroRespostaDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = "Ocorreu erro ao Salvar Usuário"
                });
            }
        }

    }
}
