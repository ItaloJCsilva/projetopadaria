using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.NotificationService.Services
{
    public class ServicoEmail : IServicoEmail
    {
        private readonly ILogger<ServicoEmail> _logger;

        public ServicoEmail(ILogger<ServicoEmail> logger)
        {
            _logger = logger;
        }

        public async Task EnviarConfirmacaoPedidoAsync(
            string emailDestino,
            string nomeCliente,
            Guid pedidoId,
            decimal total)
        {
            _logger.LogInformation(
                "📧 Email enviado para {Email} — Pedido {PedidoId} confirmado. Total: R$ {Total}",
                emailDestino,
                pedidoId,
                total);
            await Task.Delay(100);
        }
        public async Task EnviarAtualizacaoStatusAsync(
            string emailDestino,
            string nomeCliente,
            Guid pedidoId,
            string novoStatus)
        {
            _logger.LogInformation(
                "Email enviado para {Email} — Pedido {PedidoId} atualizado para: {Status}",
                emailDestino,
                pedidoId,
                novoStatus);

            await Task.Delay(100);
        }
    }
}