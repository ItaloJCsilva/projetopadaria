using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.OrderService.DTOs;
using Padaria.OrderService.Messaging;
using Padaria.OrderService.Models;
using Padaria.OrderService.Repositories.interfaces;
using Padaria.OrderService.Services.interfaces;
using Padaria.Shared.DTOS;
using Padaria.Shared.Enums;
using Padaria.Shared.Eventos;

namespace Padaria.OrderService.Services
{
    public class ServicoPedido : IServicoPedido
    {
        private readonly IRepositorioPedido _repositorio;
        private readonly PublicadorPedido _publicador;
        public ServicoPedido(IRepositorioPedido repositorio,PublicadorPedido publicador)
        {
            _repositorio = repositorio;
            _publicador  = publicador;
        }
        public async Task<List<PedidoRespostaDTO>> ListarTodosAsync()
        {
            var pedidos = await _repositorio.ListarTodosAsync();
            return pedidos.Select(MapearParaResposta).ToList();
        }

        public async Task<List<PedidoRespostaDTO>> ListarPorUsuarioAsync(Guid usuarioId)
        {
            var pedidos = await _repositorio.ListarPorUsuarioAsync(usuarioId);
            return pedidos.Select(MapearParaResposta).ToList();
        }

        public async Task<List<PedidoRespostaDTO>> ListarPorStatusAsync(StatusPedido status)
        {
            var pedidos = await _repositorio.ListarPorStatusAsync(status);
            return pedidos.Select(MapearParaResposta).ToList();
        }

        public async Task<PedidoRespostaDTO> BuscarPorIdAsync(Guid id)
        {
            var pedido = await _repositorio.BuscarPorIdAsync(id);
            if (pedido is null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            return MapearParaResposta(pedido);
        }
        public async Task<PedidoRespostaDTO> CriarAsync(CriarPedidoDTO requisicao)
        {
            if (!requisicao.Itens.Any())
                throw new InvalidOperationException("O pedido deve ter pelo menos um item.");

            var itens = requisicao.Itens.Select(i => new ItemPedido
            {
                Id = Guid.NewGuid(),
                ProdutoId = i.ProdutoId,
                NomeProduto = i.NomeProduto,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                Subtotal = i.Quantidade * i.PrecoUnitario
            }).ToList();
            var total = itens.Sum(i => i.Subtotal);

            var pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                UsuarioId = requisicao.UsuarioId,
                NomeCliente = requisicao.NomeCliente,
                EmailCliente = requisicao.EmailCliente,
                TelefoneCliente = requisicao.TelefoneCliente,
                Tipo = requisicao.Tipo,
                Status  = StatusPedido.Pendente,
                Total = total,
                Observacoes = requisicao.Observacoes,
                DataCriacao = DateTime.UtcNow,
                Itens = itens
            };

            await _repositorio.AdicionarAsync(pedido);
            await _repositorio.SalvarAsync();
            await _publicador.PublicarPedidoCriadoAsync(new PedidoCriadoEvento
            {
                PedidoId  = pedido.Id,
                NomeCliente  = pedido.NomeCliente,
                EmailCliente = pedido.EmailCliente,
                TelefoneCliente = pedido.TelefoneCliente,
                Tipo = pedido.Tipo,
                Total = pedido.Total,
                DataCriacao  = pedido.DataCriacao,
                Itens = itens.Select(i => new ItemPedidoDTOMensagem
                {
                    ProdutoId = i.ProdutoId,
                    NomeProduto = i.NomeProduto,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            });

            return MapearParaResposta(pedido);
        }
    public async Task<PedidoRespostaDTO> AtualizarStatusAsync(
        Guid id,
        AtualizarStatusDTO requisicao)
    {
        var pedido = await _repositorio.BuscarPorIdAsync(id);
        if (pedido is null)
            throw new KeyNotFoundException("Pedido não encontrado.");
        if (pedido.Status == StatusPedido.Cancelado ||
            pedido.Status == StatusPedido.Concluido)
            throw new InvalidOperationException(
                "Não é possível alterar o status de um pedido finalizado.");

        var statusAnterior = pedido.Status;
        pedido.Status       = requisicao.NovoStatus;
        pedido.DataAtualizacao = DateTime.UtcNow;
        await _repositorio.AtualizarAsync(pedido);
        await _repositorio.SalvarAsync();
        await _publicador.PublicarStatusAlteradoAsync(new StatusPedidoAlteradoEvento
        {
            PedidoId = pedido.Id,
            EmailCliente = pedido.EmailCliente,
            NomeCliente = pedido.NomeCliente,
            StatusAnterior = statusAnterior,
            NovoStatus = pedido.Status,
            DataAtualizacao  = DateTime.UtcNow
        });

        return MapearParaResposta(pedido);
    }

    public async Task CancelarAsync(Guid id)
    {
        var pedido = await _repositorio.BuscarPorIdAsync(id);
        if (pedido is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        if (pedido.Status == StatusPedido.Concluido)
            throw new InvalidOperationException(
                "Não é possível cancelar um pedido já concluído.");

        pedido.Status       = StatusPedido.Cancelado;
        pedido.DataAtualizacao = DateTime.UtcNow;

        await _repositorio.AtualizarAsync(pedido);
        await _repositorio.SalvarAsync();
    }

    private PedidoRespostaDTO MapearParaResposta(Pedido pedido)
        => new()
        {
            Id = pedido.Id,
            UsuarioId = pedido.UsuarioId,
            NomeCliente = pedido.NomeCliente,
            EmailCliente = pedido.EmailCliente,
            TelefoneCliente = pedido.TelefoneCliente,
            Tipo = pedido.Tipo,
            Status = pedido.Status,
            Total = pedido.Total,
            Observacoes= pedido.Observacoes,
            DataCriacao = pedido.DataCriacao,
            DataAtualizacao = pedido.DataAtualizacao,
            Itens= pedido.Itens.Select(i => new ItemPedidoRespostaDTO
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                NomeProduto  = i.NomeProduto,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                Subtotal = i.Subtotal
            }).ToList()
        };
    }
}