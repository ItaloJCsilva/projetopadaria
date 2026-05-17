using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Padaria.ProductService.Data;
using Padaria.ProductService.Models;
using Padaria.ProductService.Repositories.interfaces;

namespace Padaria.ProductService.Repositories
{
    public class RepositorioCategoria : IRepositorioCategoria
    {
        private readonly ContextoProduto _contexto;

        public RepositorioCategoria(ContextoProduto contexto)
        {
            _contexto = contexto;
        }
        public async Task<List<Categoria>> ListarTodasAsync()
            => await _contexto.Categorias
                            .Where(c => c.Ativa)
                            .OrderBy(c => c.Nome)
                            .ToListAsync();

        public async Task<Categoria?> BuscarPorIdAsync(Guid id)
            => await _contexto.Categorias
                            .Include(c => c.Produtos)
                            .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<bool> NomeExisteAsync(string nome)
            => await _contexto.Categorias
                            .AnyAsync(c => c.Nome == nome);

        public async Task AdicionarAsync(Categoria categoria)
            => await _contexto.Categorias.AddAsync(categoria);

        public Task AtualizarAsync(Categoria categoria)
        {
            _contexto.Categorias.Update(categoria);
            return Task.CompletedTask;
        }

        public Task RemoverAsync(Categoria categoria)
        {
            _contexto.Categorias.Remove(categoria);
            return Task.CompletedTask;
        }

        public async Task SalvarAsync()
            => await _contexto.SaveChangesAsync();
    }
}