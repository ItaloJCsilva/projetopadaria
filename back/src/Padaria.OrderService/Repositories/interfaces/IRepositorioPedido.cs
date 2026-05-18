using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.OrderService.Models;
using Padaria.Shared.Enums;

namespace Padaria.OrderService.Repositories.interfaces
{
    public interface IRepositorioPedido
    {
        Task<List<Pedido>> ListarTodosAsync();
        Task<List<Pedido>> ListarPorUsuarioAsync(Guid usuarioId);
        Task<List<Pedido>> ListarPorStatusAsync(StatusPedido status);
        Task<Pedido?> BuscarPorIdAsync(Guid id);
        Task AdicionarAsync(Pedido pedido);
        Task AtualizarAsync(Pedido pedido);
        Task SalvarAsync();
    }
}