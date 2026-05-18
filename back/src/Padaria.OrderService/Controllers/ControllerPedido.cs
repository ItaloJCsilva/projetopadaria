using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Padaria.OrderService.DTOs;
using Padaria.OrderService.Services.interfaces;
using Padaria.Shared.Enums;

namespace Padaria.OrderService.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class ControllerPedido : Controller
    {
        private readonly IServicoPedido _servico;
        public ControllerPedido(IServicoPedido servico)
        {
            _servico = servico;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Atendente")]
        public async Task<IActionResult> ListarTodos()
        {
            var pedidos = await _servico.ListarTodosAsync();
            return Ok(pedidos);
        }

        [HttpGet("meus-pedidos")]
        [Authorize]
        public async Task<IActionResult> ListarMeus()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim is null)
                return Unauthorized();

            var pedidos = await _servico.ListarPorUsuarioAsync(Guid.Parse(idClaim));
            return Ok(pedidos);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Administrador,Atendente")]
        public async Task<IActionResult> ListarPorStatus(StatusPedido status)
        {
            var pedidos = await _servico.ListarPorStatusAsync(status);
            return Ok(pedidos);
        }
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var pedido = await _servico.BuscarPorIdAsync(id);
                return Ok(pedido);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoDTO requisicao)
        {
            try
            {
                var pedido = await _servico.CriarAsync(requisicao);
                return CreatedAtAction(nameof(BuscarPorId),
                    new { id = pedido.Id }, pedido);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Administrador,Atendente")]
        public async Task<IActionResult> AtualizarStatus(
            Guid id,
            [FromBody] AtualizarStatusDTO requisicao)
        {
            try
            {
                var pedido = await _servico.AtualizarStatusAsync(id, requisicao);
                return Ok(pedido);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            try
            {
                await _servico.CancelarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    
    }
}