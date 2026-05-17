using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Padaria.ProductService.Models;

namespace Padaria.ProductService.Mapeamentos
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            
            builder.ToTable("Categorias");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedNever();
            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nome");
            builder.Property(c => c.Descricao)
                .IsRequired(false)
                .HasMaxLength(255)
                .HasColumnName("descricao");
            builder.Property(c => c.Ativa)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("ativa");

            builder.Property(c => c.DataCriacao)
                .IsRequired()
                .HasDefaultValueSql("NOW()")
                .HasColumnName("criado_em");
            builder.HasIndex(c => c.Nome)
                .IsUnique()
                .HasDatabaseName("idx_categorias_nome");
        }
    }
}