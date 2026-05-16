using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Padaria.AuthService.Models;

namespace Padaria.AuthService.Mapeamentos
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
               .IsRequired()
               .ValueGeneratedNever();
        builder.Property(u => u.NomeUsuario)
               .IsRequired()
               .HasMaxLength(50)
               .HasColumnName("nome_usuario");

        builder.Property(u => u.Nome)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("nome");

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(150)
               .HasColumnName("email");
        builder.Property(u => u.SenhaHash)
               .IsRequired()
               .HasMaxLength(255)
               .HasColumnName("senha_hash");

        builder.Property(u => u.Telefone)
               .IsRequired(false)
               .HasMaxLength(20)
               .HasColumnName("telefone");
        builder.Property(u => u.Perfil)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(30)
               .HasColumnName("perfil");
        builder.Property(u => u.DataCriacao)
               .IsRequired()
               .HasDefaultValueSql("NOW()")
               .HasColumnName("criado_em");
        builder.Property(u => u.DataAtualizacao)
               .IsRequired(false)
               .HasColumnName("atualizado_em");
        builder.Property(u => u.Ativo)
               .IsRequired()
               .HasDefaultValue(true)
               .HasColumnName("ativo");
       /*builder.HasIndex(u => u.Email)
               .IsUnique()
               .HasDatabaseName("idx_usuarios_email");
        builder.HasIndex(u => u.NomeUsuario)
               .IsUnique()
               .HasDatabaseName("idx_usuarios_nome_usuario");*/
    }
    }
}