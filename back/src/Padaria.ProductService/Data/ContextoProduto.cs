using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Padaria.ProductService.Models;

namespace Padaria.ProductService.Data
{
    public class ContextoProduto : DbContext
    {
        public ContextoProduto(DbContextOptions<ContextoProduto> opcoes)
        : base(opcoes) { }
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Produto> Produtos => Set<Produto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ContextoProduto).Assembly);
        }
    }
}