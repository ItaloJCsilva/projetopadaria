using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.Shared.DTOS;
using Padaria.Shared.Enums;

namespace Padaria.Shared.Eventos
{
    public class PedidoCriadoEvento
    {
        public Guid PedidoId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string EmailCliente { get; set; } = string.Empty;
        public string TelefoneCliente { get; set; } = string.Empty;
        public TipoPedido Tipo { get; set; }
        public decimal Total { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; } = new();
        public DateTime DataCriacao { get; set; }
    }
}