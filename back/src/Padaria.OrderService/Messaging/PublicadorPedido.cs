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

    public async Task PublicarPedidoCriadoAsync(PedidoCriadoEvento evento)
        => await _publicador.Publish(evento);

    public async Task PublicarStatusAlteradoAsync(StatusPedidoAlteradoEvento evento)
        => await _publicador.Publish(evento);

    public async Task PublicarPedidoConcluidoAsync(PedidoConcluidoEvento evento)
    {
        await _publicador.Publish(evento);
    }
    }
    
}