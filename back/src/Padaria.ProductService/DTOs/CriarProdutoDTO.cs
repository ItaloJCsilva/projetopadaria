using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.ProductService.DTOs
{
    public class CriarProdutoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public string UrlImagem { get; set; } = string.Empty;
        public Guid CategoriaId { get; set; }
    }
}