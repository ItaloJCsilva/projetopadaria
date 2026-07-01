using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Padaria.AuthService.DTOs;
using Padaria.AuthService.Models;
using Padaria.AuthService.Repositories.interfaces;
using Padaria.AuthService.Services.interfaces;
using Padaria.Shared.DTOS;
using Padaria.Shared.Enums;

namespace Padaria.AuthService.Services
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IRepositorioUsuario _repositorio;
        private readonly IConfiguration _configuracao;

        public ServicoAutenticacao(IRepositorioUsuario repositorio,IConfiguration configuracao)
        {
            _repositorio = repositorio;
            _configuracao = configuracao;
        }

    public async Task<LoginRespostaDTO> EntrarAsync(LoginRequisicaoDTO requisicao)
    {
        var usuario = await _repositorio.BuscarPorEmailAsync(requisicao.Email);
        if (usuario is null || !usuario.Ativo)
            throw new UnauthorizedAccessException("Email ou senha inválidos.");
        var senhaCorreta = BCrypt.Net.BCrypt.Verify(requisicao.Senha, usuario.SenhaHash);
        if (!senhaCorreta)
            throw new UnauthorizedAccessException("Email ou senha inválidos.");
        var token = GerarToken(usuario);
        var expiracao = DateTime.UtcNow.AddHours(24);
        return new LoginRespostaDTO
        {
            Token= token,
            Expiracao = expiracao,
            NomeUsuario = usuario.NomeUsuario,
            Nome = usuario.Nome,
            Perfil = usuario.Perfil.ToString()
        };
    }
    public async Task<UsuarioDTO> CadastrarAsync(CadastroRequisicaoDTO requisicao)
    {
        if (await _repositorio.EmailExisteAsync(requisicao.Email))
            throw new InvalidOperationException("Este email já está cadastrado.");
        if (await _repositorio.NomeUsuarioExisteAsync(requisicao.NomeUsuario))
            throw new InvalidOperationException("Este nome de usuário já está em uso.");

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(requisicao.Senha, 12);
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            NomeUsuario = requisicao.NomeUsuario,
            Nome= requisicao.Nome,
            Email= requisicao.Email,
            SenhaHash = senhaHash,
            Telefone = requisicao.Telefone,
            Perfil = PerfilUsuario.Atendente,
            DataCriacao= DateTime.UtcNow.AddHours(-3),
            Ativo = true
        };
        Console.WriteLine($"ANTES DE SALVAR: {usuario.Perfil}");
        await _repositorio.AdicionarAsync(usuario);
        Console.WriteLine($"DEPOIS DO AddAsync: {usuario.Perfil}");
        await _repositorio.SalvarAsync();
        Console.WriteLine($"DEPOIS DO SaveChanges: {usuario.Perfil}");
        return MapearParaTransferencia(usuario);
    }
    public async Task<UsuarioDTO> BuscarPerfilAsync(Guid usuarioId)
    {
        var usuario = await _repositorio.BuscarPorIdAsync(usuarioId);
        if (usuario is null)
            throw new KeyNotFoundException("Usuário não encontrado.");
        return MapearParaTransferencia(usuario);
    }
    public async Task<UsuarioDTO> AtualizarPerfilAsync(
        Guid usuarioId,
        AtualizarPerfilRequisicaoDTO requisicao)
    {
        var usuario = await _repositorio.BuscarPorIdAsync(usuarioId);
        if (usuario is null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        usuario.Nome = requisicao.Nome;
        usuario.Telefone = requisicao.Telefone;
        usuario.DataAtualizacao = DateTime.UtcNow;
        await _repositorio.AtualizarAsync(usuario);
        await _repositorio.SalvarAsync();
        return MapearParaTransferencia(usuario);
    }
    private UsuarioDTO MapearParaTransferencia(Usuario usuario)
        => new()
        {
            Id = usuario.Id,
            NomeUsuario = usuario.NomeUsuario,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Telefone = usuario.Telefone,
            Perfil = usuario.Perfil,
            CriadoEm = usuario.DataCriacao
        };
    private string GerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.NomeUsuario),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
        };
        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuracao["Jwt:Segredo"]!));
        var credenciais = new SigningCredentials(
            chave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuracao["Jwt:Emissor"],
            audience: _configuracao["Jwt:Audiencia"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credenciais
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    }
}