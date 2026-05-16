using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Padaria.AuthService.Models;

namespace Padaria.AuthService.Data;
    public class ContextoAutenticacao : DbContext
    {
    public ContextoAutenticacao(DbContextOptions<ContextoAutenticacao> opcoes)
        : base(opcoes) { }
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ContextoAutenticacao).Assembly);       
    }
}