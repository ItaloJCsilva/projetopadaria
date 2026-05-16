using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.Shared.Eventos
{
    public class EstoqueAtualizadoEvento
    {
        public Guid ProdutoId { get; set; }
        public int QuantidadeReduzida { get; set; }
        public Guid PedidoId { get; set; }
        public DateTime DataProcessamento { get; set; }
    }
}