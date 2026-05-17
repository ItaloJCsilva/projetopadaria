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
    [Route("api/produtos")]
    public class ControllerProduto : Controller
    {
        private readonly IServicoProduto _servico;
        public ControllerProduto(IServicoProduto servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var produtos = await _servico.ListarTodosAsync();
            return Ok(produtos);
        }
        [HttpGet("categoria/{categoriaId:guid}")]
        public async Task<IActionResult> ListarPorCategoria(Guid categoriaId)
        {
            var produtos = await _servico.ListarPorCategoriaAsync(categoriaId);
            return Ok(produtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var produto = await _servico.BuscarPorIdAsync(id);
                return Ok(produto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Criar([FromBody] CriarProdutoDTO requisicao)
        {
            try
            {
                var produto = await _servico.CriarAsync(requisicao);
                return CreatedAtAction(nameof(BuscarPorId),
                    new { id = produto.Id }, produto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarProdutoDTO requisicao)
        {
            try
            {
                var produto = await _servico.AtualizarAsync(id, requisicao);
                return Ok(produto);
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