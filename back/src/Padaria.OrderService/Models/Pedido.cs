using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.Shared.Enums;

namespace Padaria.OrderService.Models
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public Guid? UsuarioId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string EmailCliente { get; set; } = string.Empty;
        public string TelefoneCliente { get; set; } = string.Empty;
        public TipoPedido Tipo { get; set; }
        public StatusPedido Status { get; set; }
        public decimal Total { get; set; }
        public string? Observacoes { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<ItemPedido> Itens { get; set; } = new();
    }
}