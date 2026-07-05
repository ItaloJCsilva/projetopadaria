using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Padaria.Shared.Eventos;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;
using Padaria.Shared.Eventos;
using Padaria.NotificationService.Config;

namespace Padaria.NotificationService.Services
{
    public class ServicoEmail : IServicoEmail
    {
        private readonly ILogger<ServicoEmail> _logger;
        private readonly ConfiguracaoEmail _configuracao;
        public ServicoEmail(
            ILogger<ServicoEmail> logger,
            IOptions<ConfiguracaoEmail> configuracao)
        {
            _logger = logger;
            _configuracao = configuracao.Value;
        }
        public async Task EnviarConfirmacaoPedidoAsync(
            string emailDestino,
            string nomeCliente,
            Guid pedidoId,
            decimal total)
        {
            var html = $@"
            <html>
            <body style='font-family:Arial,Helvetica,sans-serif'>

                <h2 style='color:#d97706'>
                    🍞 Padaria
                </h2>

                <p>
                    Olá <strong>{nomeCliente}</strong>,
                </p>

                <p>
                    Recebemos seu pedido com sucesso.
                </p>

                <p>
                    <strong>Número do pedido:</strong><br>
                    {pedidoId}
                </p>

                <p>
                    <strong>Total:</strong><br>
                    R$ {total:F2}
                </p>

                <p>
                    Em breve iniciaremos o preparo.
                </p>

            </body>
            </html>";

            await EnviarEmailAsync(
                emailDestino,
                "Recebemos seu pedido",
                html);

            _logger.LogInformation(
                "Confirmação enviada para {Email}",
                emailDestino);
        }
        public async Task EnviarAtualizacaoStatusAsync(
            string emailDestino,
            string nomeCliente,
            Guid pedidoId,
            string novoStatus)
        {
            var html = $@"
            <html>

            <body style='font-family:Arial'>

                <h2 style='color:#d97706'>
                    Atualização do Pedido
                </h2>

                <p>

                    Olá
                    <strong>{nomeCliente}</strong>

                </p>

                <p>

                    O status do seu pedido foi atualizado.

                </p>

                <p>

                    <strong>Pedido</strong>

                    <br>

                    {pedidoId}

                </p>

                <p>

                    <strong>Novo Status</strong>

                    <br>

                    {novoStatus}

                </p>

            </body>

            </html>";

            await EnviarEmailAsync(
                emailDestino,
                "Atualização do Pedido",
                html);

            _logger.LogInformation(
                "Atualização enviada para {Email}",
                emailDestino);
        }

        private async Task EnviarEmailAsync(
            string destinatario,
            string assunto,
            string html)
        {
            var mensagem = new MimeMessage();

            mensagem.From.Add(
                new MailboxAddress(
                    _configuracao.NomeRemetente,
                    _configuracao.Email));

            mensagem.To.Add(
                MailboxAddress.Parse(destinatario));

            mensagem.Subject = assunto;

            mensagem.Body = new BodyBuilder
            {
                HtmlBody = html
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _configuracao.Servidor,
                _configuracao.Porta,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuracao.Email,
                _configuracao.Senha);

            await smtp.SendAsync(mensagem);

            await smtp.DisconnectAsync(true);
        }

        private string GerarHtmlRecibo(
            PedidoConcluidoEvento pedido)
        {
            var linhas = new StringBuilder();

            foreach (var item in pedido.Itens)
            {
                linhas.Append($@"
        <tr>
        <td>{item.NomeProduto}</td>
        <td align='center'>{item.Quantidade}</td>
        <td align='right'>
        R$ {item.PrecoUnitario:F2}
        </td>
        <td align='right'>
        R$ {item.Subtotal:F2}
        </td>
        </tr>");
            }

            return $@"
        <html>

        <body style='font-family:Arial'>

        <h2>
         Padaria
        </h2>

        <p>

        Olá <strong>{pedido.NomeCliente}</strong>,

        </p>

        <p>

        Seu pedido foi concluído com sucesso.

        </p>

        <hr>

        <p>

        Pedido:
        <b>{pedido.PedidoId}</b>

        </p>

        <p>

        Data:

        {pedido.DataConclusao:dd/MM/yyyy HH:mm}

        </p>

        <table
        border='1'
        cellpadding='8'
        cellspacing='0'
        width='100%'>

        <tr>

        <th>Produto</th>

        <th>Qtd</th>

        <th>Unitário</th>

        <th>Subtotal</th>

        </tr>

        {linhas}

        </table>

        <h2>

        Total:
        R$ {pedido.Total:F2}

        </h2>

        <hr>

        <p>

        Obrigado pela preferência!

        </p>

        </body>

        </html>";
        }

        public async Task EnviarReciboPedidoAsync(
            PedidoConcluidoEvento pedido)
        {
            var html = GerarHtmlRecibo(pedido);

            await EnviarEmailAsync(
                pedido.EmailCliente,
                "Seu pedido foi concluído",
                html);

            await EnviarEmailAsync(
                _configuracao.EmailAdministrador,
                $"Pedido concluído - {pedido.PedidoId}",
                html);

            _logger.LogInformation(
                "Recibo enviado para {Email}",
                pedido.EmailCliente);
        }

    }
}