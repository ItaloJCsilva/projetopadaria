using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.AuthService.Models;
namespace Padaria.AuthService.Repositories.interfaces
{
    public interface IRepositorioUsuario
    {
        Task<Usuario?> BuscarPorIdAsync(Guid id);
    Task<Usuario?> BuscarPorEmailAsync(string email);
    Task<Usuario?> BuscarPorNomeUsuarioAsync(string nomeUsuario);
    Task<bool> EmailExisteAsync(string email);
    Task<bool> NomeUsuarioExisteAsync(string nomeUsuario);
    Task AdicionarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task SalvarAsync();
    }
}