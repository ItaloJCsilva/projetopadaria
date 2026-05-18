using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.Shared.Enums;

namespace Padaria.Shared.DTOS
{
    public class PedidoDTO
    {
        public Guid Id { get; set; }
        public Guid? UsuarioId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string EmailCliente { get; set; } = string.Empty;
        public TipoPedido Tipo { get; set; }
        public StatusPedido Status { get; set; }
        public decimal Total { get; set; }
        public string? Observacoes { get; set; }
        public List<ItemPedidoDTOMensagem> Itens { get; set; } = new();
        public DateTime DataCriacao { get; set; }
    }
}