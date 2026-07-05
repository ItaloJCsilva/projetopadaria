using MassTransit;
using Padaria.NotificationService.Services;
using Padaria.Shared.Eventos;

namespace Padaria.NotificationService.Consumers
{
    public class ConsumidorPedidoConcluido : IConsumer<PedidoConcluidoEvento>
    {
        private readonly IServicoEmail _servicoEmail;
        private readonly ILogger<ConsumidorPedidoConcluido> _logger;

        public ConsumidorPedidoConcluido(
            IServicoEmail servicoEmail,
            ILogger<ConsumidorPedidoConcluido> logger)
        {
            _servicoEmail = servicoEmail;
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<PedidoConcluidoEvento> contexto)
        {
            var evento = contexto.Message;

            _logger.LogInformation(
                "Pedido concluído recebido. Pedido {PedidoId}",
                evento.PedidoId);

            await _servicoEmail.EnviarReciboPedidoAsync(evento);
        }
    }
}