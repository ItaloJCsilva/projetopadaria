// O using do Microsoft.EntityFrameworkCore é obrigatório
// para ter acesso aos métodos assíncronos:
// FirstOrDefaultAsync, AnyAsync, AddAsync, SaveChangesAsync
// Sem ele o compilador não encontra esses métodos de extensão
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

    // Busca usuário pelo Id — retorna null se não encontrar
    public async Task<Usuario?> BuscarPorIdAsync(Guid id)
        => await _contexto.Usuarios
                          .FirstOrDefaultAsync(u => u.Id == id);

    // Busca usuário pelo email — usado no login
    public async Task<Usuario?> BuscarPorEmailAsync(string email)
        => await _contexto.Usuarios
                          .FirstOrDefaultAsync(u => u.Email == email);

    // Busca usuário pelo nome de usuário
    public async Task<Usuario?> BuscarPorNomeUsuarioAsync(string nomeUsuario)
        => await _contexto.Usuarios
                          .FirstOrDefaultAsync(u => u.NomeUsuario == nomeUsuario);

    // Verifica se o email já está cadastrado
    public async Task<bool> EmailExisteAsync(string email)
        => await _contexto.Usuarios
                          .AnyAsync(u => u.Email == email);

    // Verifica se o nome de usuário já está em uso
    public async Task<bool> NomeUsuarioExisteAsync(string nomeUsuario)
        => await _contexto.Usuarios
                          .AnyAsync(u => u.NomeUsuario == nomeUsuario);

    // Adiciona novo usuário ao contexto
    // Ainda não salva no banco — precisa chamar SalvarAsync()
    public async Task AdicionarAsync(Usuario usuario)
        => await _contexto.Usuarios.AddAsync(usuario);

    // Marca o usuário como modificado no contexto
    public Task AtualizarAsync(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    // Persiste todas as mudanças pendentes no banco
    public async Task SalvarAsync()
        => await _contexto.SaveChangesAsync();
}