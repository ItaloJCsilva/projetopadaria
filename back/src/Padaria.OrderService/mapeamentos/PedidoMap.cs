using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Padaria.OrderService.Models;

namespace Padaria.OrderService.mapeamentos
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedNever();
            builder.Property(p => p.UsuarioId)
                .IsRequired(false)
                .HasColumnName("usuario_id");
            builder.Property(p => p.NomeCliente)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("nome_cliente");
            builder.Property(p => p.EmailCliente)
                .IsRequired(false)
                .HasMaxLength(150)
                .HasColumnName("email_cliente");
            builder.Property(p => p.TelefoneCliente)
                .IsRequired(false)
                .HasMaxLength(20)
                .HasColumnName("telefone_cliente");
            builder.Property(p => p.Tipo)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnName("tipo");
            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnName("status");
            builder.Property(p => p.Total)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasColumnName("total");
            builder.Property(p => p.Observacoes)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnName("observacoes");

            builder.Property(p => p.DataCriacao)
                .IsRequired()
                .HasDefaultValueSql("NOW()")
                .HasColumnName("criado_em");
            builder.Property(p => p.DataAtualizacao)
                .IsRequired(false)
                .HasColumnName("atualizado_em");
            builder.HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(p => p.UsuarioId)
                .HasDatabaseName("idx_pedidos_usuario_id");
            builder.HasIndex(p => p.Status)
                .HasDatabaseName("idx_pedidos_status");
            builder.HasIndex(p => p.DataCriacao)
                .HasDatabaseName("idx_pedidos_criado_em");
        }
    }
}