using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.Shared.Enums;

namespace Padaria.Shared.Eventos
{
    public class StatusPedidoAlteradoEvento
    {
    public Guid PedidoId { get; set; }    
    public string EmailCliente { get; set; } = string.Empty;
    public string NomeCliente { get; set; } = string.Empty;
    public StatusPedido StatusAnterior { get; set; }
    public StatusPedido NovoStatus { get; set; }
    public DateTime DataAlteracao { get; set; }
    }
}