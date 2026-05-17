using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.ProductService.Models
{
    public class Categoria
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Ativa { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public List<Produto> Produtos { get; set; } = new();
    }
}