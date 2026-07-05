using MassTransit;
using Padaria.NotificationService.Services;
using Padaria.Shared.Eventos;

namespace Padaria.NotificationService.Consumers
{
    public class ConsumidorStatusPedidoAlterado : IConsumer<StatusPedidoAlteradoEvento>
    {
        private readonly IServicoEmail _servicoEmail;
        private readonly ILogger<ConsumidorStatusPedidoAlterado> _logger;

        public ConsumidorStatusPedidoAlterado(
            IServicoEmail servicoEmail,
            ILogger<ConsumidorStatusPedidoAlterado> logger)
        {
            _servicoEmail = servicoEmail;
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<StatusPedidoAlteradoEvento> contexto)
        {
            var evento = contexto.Message;

            _logger.LogInformation(
                "Pedido {PedidoId} alterado para {NovoStatus}",
                evento.PedidoId,
                evento.NovoStatus);

            if (string.IsNullOrWhiteSpace(evento.EmailCliente))
                return;

            await _servicoEmail.EnviarAtualizacaoStatusAsync(
                emailDestino: evento.EmailCliente,
                nomeCliente: evento.NomeCliente,
                pedidoId: evento.PedidoId,
                novoStatus: evento.NovoStatus.ToString());
        }
    }
}