using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.Shared.Enums;

namespace Padaria.OrderService.DTOs
{
    public class AtualizarStatusDTO
    {
        public StatusPedido NovoStatus { get; set; }
    }
}