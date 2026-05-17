using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.ProductService.DTOs
{
    public class AtualizarProdutoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public string UrlImagem { get; set; } = string.Empty;
        public bool Disponivel { get; set; }
        public Guid CategoriaId { get; set; }
    }
}