using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Padaria.OrderService.Data;
using Padaria.OrderService.Models;
using Padaria.OrderService.Repositories.interfaces;
using Padaria.Shared.Enums;

namespace Padaria.OrderService.Repositories
{
    public class RepositorioPedido : IRepositorioPedido
    {
        private readonly ContextoPedido _contexto;

        public RepositorioPedido(ContextoPedido contexto)
        {
            _contexto = contexto;
        }
        public async Task<List<Pedido>> ListarTodosAsync()
            => await _contexto.Pedidos
                            .Include(p => p.Itens)
                            .OrderByDescending(p => p.DataCriacao)
                            .ToListAsync();
        public async Task<List<Pedido>> ListarPorUsuarioAsync(Guid usuarioId)
            => await _contexto.Pedidos
                            .Include(p => p.Itens)
                            .Where(p => p.UsuarioId == usuarioId)
                            .OrderByDescending(p => p.DataCriacao)
                            .ToListAsync();
        public async Task<List<Pedido>> ListarPorStatusAsync(StatusPedido status)
            => await _contexto.Pedidos
                            .Include(p => p.Itens)
                            .Where(p => p.Status == status)
                            .OrderBy(p => p.DataCriacao)
                            .ToListAsync();
        public async Task<Pedido?> BuscarPorIdAsync(Guid id)
            => await _contexto.Pedidos
                            .Include(p => p.Itens)
                            .FirstOrDefaultAsync(p => p.Id == id);

        public async Task AdicionarAsync(Pedido pedido)
            => await _contexto.Pedidos.AddAsync(pedido);

        public Task AtualizarAsync(Pedido pedido)
        {
            _contexto.Pedidos.Update(pedido);
            return Task.CompletedTask;
        }
        public async Task SalvarAsync()
            => await _contexto.SaveChangesAsync();
        }
}