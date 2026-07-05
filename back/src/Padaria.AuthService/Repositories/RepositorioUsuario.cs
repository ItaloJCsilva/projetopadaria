using Microsoft.EntityFrameworkCore;
using Padaria.AuthService.Data;
using Padaria.AuthService.Models;
using Padaria.AuthService.Repositories.interfaces;


namespace Padaria.AuthService.Repositorios;

public class RepositorioUsuario : IRepositorioUsuario
{
    private readonly ContextoAutenticacao _contexto;

    public RepositorioUsuario(ContextoAutenticacao contexto)
    {
        _contexto = contexto;
    }

    public async Task<Usuario?> BuscarPorIdAsync(Guid id)
        => await _contexto.Usuarios
                          .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
        => await _contexto.Usuarios
                          .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<Usuario?> BuscarPorNomeUsuarioAsync(string nomeUsuario)
        => await _contexto.Usuarios
                          .FirstOrDefaultAsync(u => u.NomeUsuario == nomeUsuario);

    public async Task<bool> EmailExisteAsync(string email)
        => await _contexto.Usuarios
                          .AnyAsync(u => u.Email == email);
    public async Task<bool> NomeUsuarioExisteAsync(string nomeUsuario)
        => await _contexto.Usuarios
                          .AnyAsync(u => u.NomeUsuario == nomeUsuario);
    public async Task AdicionarAsync(Usuario usuario)
        => await _contexto.Usuarios.AddAsync(usuario);

    public Task AtualizarAsync(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public async Task SalvarAsync()
        => await _contexto.SaveChangesAsync();
}