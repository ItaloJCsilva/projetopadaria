using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Padaria.AuthService.DTOs;
using Padaria.AuthService.Repositories.interfaces;
using Padaria.AuthService.Services.interfaces;

namespace Padaria.AuthService.Controllers
{
    [ApiController]
    [Route("api/autenticacao")]
    public class ControladorAutenticacao : Controller
    {
        private readonly IServicoAutenticacao _servico;

        public ControladorAutenticacao(IServicoAutenticacao servico)
        {
            _servico = servico;
        }
    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastroRequisicaoDTO requisicao)
    {
        try
        {
            var usuario = await _servico.CadastrarAsync(requisicao);
            return CreatedAtAction(nameof(BuscarPerfil),
                new { id = usuario.Id }, usuario);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Entrar([FromBody] LoginRequisicaoDTO requisicao)
    {
        try
        {
            var resposta = await _servico.EntrarAsync(requisicao);
            return Ok(resposta);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensagem = ex.Message });
        }
    }
    [HttpGet("Pegar-perfil")]
    [Authorize]
    public async Task<IActionResult> BuscarPerfil()
    {
        try
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim is null)
                return Unauthorized();

            var usuario = await _servico.BuscarPerfilAsync(Guid.Parse(idClaim));
            return Ok(usuario);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
    [HttpPut("Atualizar-perfil")]
    [Authorize]
    public async Task<IActionResult> AtualizarPerfil(
        [FromBody] AtualizarPerfilRequisicaoDTO requisicao)
    {
        try
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim is null)
                return Unauthorized();

            var usuario = await _servico.AtualizarPerfilAsync(
                Guid.Parse(idClaim), requisicao);

            return Ok(usuario);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
    }
}