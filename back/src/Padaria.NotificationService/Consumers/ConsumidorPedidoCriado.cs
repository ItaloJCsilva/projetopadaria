using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Padaria.NotificationService.Services;
using Padaria.Shared.Eventos;

namespace Padaria.NotificationService.Consumers
{
    public class ConsumidorPedidoCriado : IConsumer<PedidoCriadoEvento>
    {
        private readonly IServicoEmail _servicoEmail;
        private readonly ILogger<ConsumidorPedidoCriado> _logger;
        public ConsumidorPedidoCriado(IServicoEmail servicoEmail, ILogger<ConsumidorPedidoCriado> logger)
        {
            _servicoEmail = servicoEmail;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PedidoCriadoEvento> contexto)
        {
            var evento = contexto.Message;

            _logger.LogInformation(
                "Pedido recebido — Id: {PedidoId} | Cliente: {NomeCliente} | Total: R$ {Total}",
                evento.PedidoId,
                evento.NomeCliente,
                evento.Total);

            if (!string.IsNullOrEmpty(evento.EmailCliente))
            {
                await _servicoEmail.EnviarConfirmacaoPedidoAsync(
                    emailDestino: evento.EmailCliente,
                    nomeCliente:  evento.NomeCliente,
                    pedidoId: evento.PedidoId,
                    total:evento.Total);
            }
            else
            {
                _logger.LogInformation(
                    "Pedido {PedidoId} sem email — notificação não enviada.",
                    evento.PedidoId);
            }
        }
    }
}