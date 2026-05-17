using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.ProductService.Models;

namespace Padaria.ProductService.Repositories.interfaces
{
    public interface IRepositorioProduto
    {
        Task<List<Produto>> ListarTodosAsync();
        Task<List<Produto>> ListarPorCategoriaAsync(Guid categoriaId);
        Task<Produto?> BuscarPorIdAsync(Guid id);
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task RemoverAsync(Produto produto);
        Task SalvarAsync();
    }
}