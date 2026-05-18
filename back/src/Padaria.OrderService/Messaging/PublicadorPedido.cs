using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Padaria.Shared.Eventos;

namespace Padaria.OrderService.Messaging
{
    public class PublicadorPedido
    {
        private readonly IPublishEndpoint _publicador;

    public PublicadorPedido(IPublishEndpoint publicador)
    {
        _publicador = publicador;
    }
    // Publica o evento quando um pedido é criado
    // ServiçoNotificacao consome e envia email ao cliente
    public async Task PublicarPedidoCriadoAsync(PedidoCriadoEvento evento)
        => await _publicador.Publish(evento);
    // Publica o evento quando o status muda
    // Ex: de Confirmado para Pronto — avisa cliente
    public async Task PublicarStatusAlteradoAsync(StatusPedidoAlteradoEvento evento)
        => await _publicador.Publish(evento);
    }
}