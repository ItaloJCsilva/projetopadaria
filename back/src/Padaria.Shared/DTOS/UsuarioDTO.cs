using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.Shared.Enums;

namespace Padaria.Shared.DTOS
{
    public class UsuarioDTO

    {
        public Guid Id { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public PerfilUsuario Perfil { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}