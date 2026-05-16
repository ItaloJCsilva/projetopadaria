using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.AuthService.DTOs;
using Padaria.Shared.DTOS;

namespace Padaria.AuthService.Services.interfaces
{
    public interface IServicoAutenticacao
    {
        Task<LoginRespostaDTO> EntrarAsync(LoginRequisicaoDTO requisicao);
        Task<UsuarioDTO> CadastrarAsync(CadastroRequisicaoDTO requisicao);
        Task<UsuarioDTO> BuscarPerfilAsync(Guid usuarioId);
        Task<UsuarioDTO> AtualizarPerfilAsync(Guid usuarioId, AtualizarPerfilRequisicaoDTO requisicao);
    }
}