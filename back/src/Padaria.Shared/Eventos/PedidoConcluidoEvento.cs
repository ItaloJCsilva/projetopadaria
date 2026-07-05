using Padaria.Shared.DTOS;

namespace Padaria.Shared.Eventos
{
    public class PedidoConcluidoEvento
    {
        public Guid PedidoId { get; set; }

        public Guid? UsuarioId { get; set; }

        public string NomeCliente { get; set; } = string.Empty;

        public string EmailCliente { get; set; } = string.Empty;

        public string TelefoneCliente { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public DateTime DataConclusao { get; set; }

        public List<ItemPedidoDTOMensagem> Itens { get; set; } = new();
    }
}