﻿using GerenciadorDeTarefas.Dtos;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository;
using GerenciadorDeTarefas.Services;
using GerenciadorDeTarefas.Utils;
using Microsoft.AspNetCore.Authorization;
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
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

     
        

        public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository): base(usuarioRepository)
        {
            _logger = logger;
            
        }

        [HttpPost]
        [AllowAnonymous]

        public IActionResult EfetuarLogin([FromBody] LoginRequisicaoDto requisicao)
        {
            try
            {
                if (requisicao == null || 
                    (string.IsNullOrEmpty(requisicao.Login)|| string.IsNullOrWhiteSpace(requisicao.Login))
                    ||(string.IsNullOrEmpty( requisicao.Senha) || string.IsNullOrWhiteSpace(requisicao.Senha)))
                {
                    return BadRequest(new ErroRespostaDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Usuário ou Senha inválidos!"
                    });
                }
                var usuario = _usuarioRepository.GetUsuarioByLoginSenha(requisicao.Login, MD5Utils.GerarHashMD5(requisicao.Senha));

              

                var token = TokenService.CriarToken(usuario);
                return Ok(new LoginRespostaDto()
                {
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    Token = token

                });
                

            }
            catch (Exception excecao)

            {
                _logger.LogError($"Ocorreu erro no Login: Login = {requisicao.Login} e Senha = {requisicao.Senha}", excecao);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErroRespostaDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = "Ocorreu erro ao efetuar o Login"
                });

            }
        }
    }
}
