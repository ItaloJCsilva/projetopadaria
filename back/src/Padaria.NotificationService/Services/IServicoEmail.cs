using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.NotificationService.Services
{
    public interface IServicoEmail
    {
        Task EnviarConfirmacaoPedidoAsync(
        string emailDestino,
        string nomeCliente,
        Guid pedidoId,
        decimal total);
        Task EnviarAtualizacaoStatusAsync(
            string emailDestino,
            string nomeCliente,
            Guid pedidoId,
            string novoStatus);
    }
}