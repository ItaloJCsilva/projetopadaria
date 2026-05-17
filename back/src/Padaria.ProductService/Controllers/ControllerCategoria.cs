using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Padaria.ProductService.DTOs;
using Padaria.ProductService.Services.interfaces;

namespace Padaria.ProductService.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class ControllerCategoria : Controller
    {
        private readonly IServicoCategoria _servico;

        public ControllerCategoria(IServicoCategoria servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var categorias = await _servico.ListarTodasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var categoria = await _servico.BuscarPorIdAsync(id);
                return Ok(categoria);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Criar([FromBody] CriarCategoriaDTO requisicao)
        {
            try
            {
                var categoria = await _servico.CriarAsync(requisicao);
                return CreatedAtAction(nameof(BuscarPorId),
                    new { id = categoria.Id }, categoria);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CriarCategoriaDTO requisicao)
        {
            try
            {
                var categoria = await _servico.AtualizarAsync(id, requisicao);
                return Ok(categoria);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                await _servico.RemoverAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
        
    }
}