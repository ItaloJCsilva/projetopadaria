using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.ProductService.DTOs;
using Padaria.ProductService.Models;
using Padaria.ProductService.Repositories.interfaces;
using Padaria.ProductService.Services.interfaces;

namespace Padaria.ProductService.Services
{
    public class ServicoProduto : IServicoProduto
    {
        private readonly IRepositorioProduto _repositorio;
        private readonly IRepositorioCategoria _repositorioCategoria;

        public ServicoProduto(IRepositorioProduto repositorio,IRepositorioCategoria repositorioCategoria)
        {
            _repositorio         = repositorio;
            _repositorioCategoria = repositorioCategoria;
        }
        public async Task<List<ProdutoDTO>> ListarTodosAsync()
        {
            var produtos = await _repositorio.ListarTodosAsync();
            return produtos.Select(MapearParaResposta).ToList();
        }
        public async Task<List<ProdutoDTO>> ListarPorCategoriaAsync(Guid categoriaId)
        {
            var produtos = await _repositorio.ListarPorCategoriaAsync(categoriaId);
            return produtos.Select(MapearParaResposta).ToList();
        }
        public async Task<ProdutoDTO> BuscarPorIdAsync(Guid id)
        {
            var produto = await _repositorio.BuscarPorIdAsync(id);
            if (produto is null)
                throw new KeyNotFoundException("Produto não encontrado.");

            return MapearParaResposta(produto);
        }
        public async Task<ProdutoDTO> CriarAsync(CriarProdutoDTO requisicao, string? urlImagem)
        {            
            var categoria = await _repositorioCategoria.BuscarPorIdAsync(requisicao.CategoriaId);
            if (categoria is null)
                throw new KeyNotFoundException("Categoria não encontrada.");

            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = requisicao.Nome,
                Descricao = requisicao.Descricao,
                Preco  = requisicao.Preco,
                Estoque = requisicao.Estoque,
                UrlImagem = urlImagem,
                CategoriaId = requisicao.CategoriaId,
                Disponivel = true,
                DataCriacao = DateTime.UtcNow
            };
            await _repositorio.AdicionarAsync(produto);
            await _repositorio.SalvarAsync();
            produto.Categoria = categoria;
            return MapearParaResposta(produto);
        }
    public async Task<ProdutoDTO> AtualizarAsync(Guid id, AtualizarProdutoDTO requisicao)
    {
        var produto = await _repositorio.BuscarPorIdAsync(id);
        if (produto is null)
            throw new KeyNotFoundException("Produto não encontrado.");
        var categoria = await _repositorioCategoria.BuscarPorIdAsync(requisicao.CategoriaId);
        if (categoria is null)
            throw new KeyNotFoundException("Categoria não encontrada.");

        produto.Nome = requisicao.Nome;
        produto.Descricao = requisicao.Descricao;
        produto.Preco = requisicao.Preco;
        produto.Estoque = requisicao.Estoque;
        produto.UrlImagem = requisicao.UrlImagem;
        produto.Disponivel = requisicao.Disponivel;
        produto.CategoriaId = requisicao.CategoriaId;
        produto.DataAtualizacao = DateTime.UtcNow;
        await _repositorio.AtualizarAsync(produto);
        await _repositorio.SalvarAsync();
        return MapearParaResposta(produto);
    }
    public async Task RemoverAsync(Guid id)
    {
        var produto = await _repositorio.BuscarPorIdAsync(id);
        if (produto is null)
            throw new KeyNotFoundException("Produto não encontrado.");
        produto.Disponivel   = false;
        produto.DataAtualizacao = DateTime.UtcNow;
        await _repositorio.AtualizarAsync(produto);
        await _repositorio.SalvarAsync();
    }
    private ProdutoDTO MapearParaResposta(Produto produto)
        => new()
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            UrlImagem = produto.UrlImagem,
            Disponivel = produto.Disponivel,
            CategoriaId = produto.CategoriaId,
            NomeCategoria = produto.Categoria?.Nome ?? string.Empty,
            DataCriacao  = produto.DataCriacao,
            DataAtualizacao = produto.DataAtualizacao
        };
    }
}