using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Padaria.NotificationService.Services;
using Padaria.Shared.Enums;
using Padaria.Shared.Eventos;

namespace Padaria.NotificationService.Consumers
{
    public class ConsumidorStatusPedidoAlterado : IConsumer<StatusPedidoAlteradoEvento>
    {
        private readonly IServicoEmail _servicoEmail;
        private readonly ILogger<ConsumidorStatusPedidoAlterado> _logger;
        public ConsumidorStatusPedidoAlterado(IServicoEmail servicoEmail,ILogger<ConsumidorStatusPedidoAlterado> logger)
        {
            _servicoEmail = servicoEmail;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<StatusPedidoAlteradoEvento> contexto)
        {
            var evento = contexto.Message;
            _logger.LogInformation(
                "Status alterado — Pedido: {PedidoId} | {StatusAnterior} → {NovoStatus}",
                evento.PedidoId,
                evento.StatusAnterior,
                evento.NovoStatus);
            var statusRelevante =
                evento.NovoStatus == StatusPedido.Confirmado ||
                evento.NovoStatus == StatusPedido.Pronto     ||
                evento.NovoStatus == StatusPedido.Cancelado;
            if (statusRelevante && !string.IsNullOrEmpty(evento.EmailCliente))
            {
                await _servicoEmail.EnviarAtualizacaoStatusAsync(
                    emailDestino: evento.EmailCliente,
                    nomeCliente:evento.NomeCliente,
                    pedidoId:evento.PedidoId,
                    novoStatus:evento.NovoStatus.ToString());
            }
        }
    }
}