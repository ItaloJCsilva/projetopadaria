using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.OrderService.DTOs;
using Padaria.Shared.Enums;

namespace Padaria.OrderService.Services.interfaces
{
    public interface IServicoPedido
    {
        Task<List<PedidoRespostaDTO>> ListarTodosAsync();
        Task<List<PedidoRespostaDTO>> ListarPorUsuarioAsync(Guid usuarioId);
        Task<List<PedidoRespostaDTO>> ListarPorStatusAsync(StatusPedido status);
        Task<PedidoRespostaDTO> BuscarPorIdAsync(Guid id);
        Task<PedidoRespostaDTO> CriarAsync(CriarPedidoDTO requisicao);
        Task<PedidoRespostaDTO> AtualizarStatusAsync(Guid id, AtualizarStatusDTO requisicao);
        Task CancelarAsync(Guid id);
    }
}