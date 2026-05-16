using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.Shared.DTOS
{
    public class ProdutoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public string NomeCategoria { get; set; } = string.Empty;
        public string UrlImagem { get; set; } = string.Empty;
        public bool Disponivel { get; set; }
    }
}