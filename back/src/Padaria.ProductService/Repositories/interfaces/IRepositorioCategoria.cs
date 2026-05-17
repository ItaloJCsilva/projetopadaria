using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.ProductService.Models;

namespace Padaria.ProductService.Repositories.interfaces
{
    public interface IRepositorioCategoria
    {
        Task<List<Categoria>> ListarTodasAsync();
        Task<Categoria?> BuscarPorIdAsync(Guid id);
        Task<bool> NomeExisteAsync(string nome);
        Task AdicionarAsync(Categoria categoria);
        Task AtualizarAsync(Categoria categoria);
        Task RemoverAsync(Categoria categoria);
        Task SalvarAsync();
    }
}