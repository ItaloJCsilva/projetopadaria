using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Padaria.OrderService.Models;

namespace Padaria.OrderService.Data
{
    public class ContextoPedido : DbContext
    {
        public ContextoPedido(DbContextOptions<ContextoPedido> opcoes)
        : base(opcoes) { }
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ContextoPedido).Assembly);
        }
    }
}