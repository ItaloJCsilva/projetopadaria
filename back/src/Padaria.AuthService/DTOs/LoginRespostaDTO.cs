using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.AuthService.DTOs
{
    public class LoginRespostaDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
    }
}