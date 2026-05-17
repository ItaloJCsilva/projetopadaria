using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Padaria.ProductService.Models;

namespace Padaria.ProductService.Mapeamentos
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
       public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedNever();
            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("nome");
            builder.Property(p => p.Descricao)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnName("descricao");
            builder.Property(p => p.Preco)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasColumnName("preco");
            builder.Property(p => p.Estoque)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnName("estoque");
            builder.Property(p => p.UrlImagem)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnName("url_imagem");
            builder.Property(p => p.Disponivel)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("disponivel");
            builder.Property(p => p.CategoriaId)
                .IsRequired()
                .HasColumnName("categoria_id");
            builder.Property(p => p.DataCriacao)
                .IsRequired()
                .HasDefaultValueSql("NOW()")
                .HasColumnName("criado_em");
            builder.Property(p => p.DataAtualizacao)
                .IsRequired(false)
                .HasColumnName("atualizado_em");
            builder.HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(p => p.Nome)
                .HasDatabaseName("idx_produtos_nome");
            builder.HasIndex(p => p.CategoriaId)
                .HasDatabaseName("idx_produtos_categoria_id");
        } 
    }
}