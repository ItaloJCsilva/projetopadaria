using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.Shared.Enums;

namespace Padaria.OrderService.DTOs
{
    public class CriarPedidoDTO
    {
        public Guid? UsuarioId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string EmailCliente { get; set; } = string.Empty;
        public string TelefoneCliente { get; set; } = string.Empty;
        public TipoPedido Tipo { get; set; }
        public string? Observacoes { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; } = new();
    }
}