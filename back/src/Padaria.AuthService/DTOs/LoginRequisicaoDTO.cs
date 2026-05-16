using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padaria.AuthService.DTOs
{
    public class LoginRequisicaoDTO
    {
         public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}