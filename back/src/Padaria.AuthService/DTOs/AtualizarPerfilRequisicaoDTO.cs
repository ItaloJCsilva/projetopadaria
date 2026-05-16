using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Padaria.AuthService.DTOs
{
    public class AtualizarPerfilRequisicaoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}