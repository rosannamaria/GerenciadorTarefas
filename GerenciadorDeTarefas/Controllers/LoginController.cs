using GerenciadorDeTarefas.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        private readonly string LoginTeste = "Admin";
        private readonly string SenhaTeste = "123";

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]

        public IActionResult EfetuarLogin([FromBody] LoginRequisicaoDto requisicao)
        {
            try
            {
                if (requisicao == null || 
                    (string.IsNullOrEmpty(requisicao.Login)|| string.IsNullOrWhiteSpace(requisicao.Login))
                    ||(string.IsNullOrEmpty( requisicao.Senha) || string.IsNullOrWhiteSpace(requisicao.Senha))
                    || requisicao.Login != LoginTeste || requisicao.Senha != SenhaTeste)
                {
                    return BadRequest(new ErroRespostaDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Parametro de entrada inválidos"
                    });
                }
                return Ok(new LoginRespostaDto()
                {
                    Email = LoginTeste,
                    Nome = "Usuário de Teste",
                    Token = ""

                });
                

            }
            catch (Exception excessao)

            {
                _logger.LogError($"Ocorreu erro no Login: Login = {requisicao.Login} e Senha = {requisicao.Senha}", excessao, requisicao);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErroRespostaDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = "Ocorreu erro ao efetuar o Login"
                });

            }
        }
    }
}
