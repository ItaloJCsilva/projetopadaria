namespace Padaria.NotificationService.Config
{
    public class ConfiguracaoEmail
    {
        public string Servidor { get; set; } = string.Empty;

        public int Porta { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public string NomeRemetente { get; set; } = string.Empty;

        public string EmailAdministrador { get; set; } = string.Empty;
    }
}