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
    public class RepositorioProduto : IRepositorioProduto
    {
        private readonly ContextoProduto _contexto;

        public RepositorioProduto(ContextoProduto contexto)
        {
            _contexto = contexto;
        }
        public async Task<List<Produto>> ListarTodosAsync()
            => await _contexto.Produtos
                            .Include(p => p.Categoria)
                            .Where(p => p.Disponivel)
                            .OrderBy(p => p.Nome)
                            .ToListAsync();
        public async Task<List<Produto>> ListarPorCategoriaAsync(Guid categoriaId)
            => await _contexto.Produtos
                            .Include(p => p.Categoria)
                            .Where(p => p.CategoriaId == categoriaId && p.Disponivel)
                            .OrderBy(p => p.Nome)
                            .ToListAsync();
        public async Task<Produto?> BuscarPorIdAsync(Guid id)
            => await _contexto.Produtos
                            .Include(p => p.Categoria)
                            .FirstOrDefaultAsync(p => p.Id == id);

        public async Task AdicionarAsync(Produto produto)
            => await _contexto.Produtos.AddAsync(produto);

        public Task AtualizarAsync(Produto produto)
        {
            _contexto.Produtos.Update(produto);
            return Task.CompletedTask;
        }
        public Task RemoverAsync(Produto produto)
        {
            _contexto.Produtos.Remove(produto);
            return Task.CompletedTask;
        }
        public async Task SalvarAsync()
            => await _contexto.SaveChangesAsync();
    }
}