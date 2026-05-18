using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Padaria.OrderService.Models;

namespace Padaria.OrderService.mapeamentos
{
    public class ItemPedidoMap : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.ToTable("ItensPedido");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .IsRequired()
                .ValueGeneratedNever();
            builder.Property(i => i.PedidoId)
                .IsRequired()
                .HasColumnName("pedido_id");
            builder.Property(i => i.ProdutoId)
                .IsRequired()
                .HasColumnName("produto_id");
            builder.Property(i => i.NomeProduto)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("nome_produto");
            builder.Property(i => i.Quantidade)
                .IsRequired()
                .HasColumnName("quantidade");
            builder.Property(i => i.PrecoUnitario)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasColumnName("preco_unitario");
            builder.Property(i => i.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasColumnName("subtotal");
            builder.HasIndex(i => i.PedidoId)
                .HasDatabaseName("idx_itens_pedido_id");
        }
    }
}