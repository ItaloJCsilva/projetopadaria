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
    public class ServicoCategoria : IServicoCategoria   
    {
        private readonly IRepositorioCategoria _repositorio;

        public ServicoCategoria(IRepositorioCategoria repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<CategoriaDTO>> ListarTodasAsync()
        {
            var categorias = await _repositorio.ListarTodasAsync();
            return categorias.Select(MapearParaResposta).ToList();
        }

        public async Task<CategoriaDTO> BuscarPorIdAsync(Guid id)
        {
            var categoria = await _repositorio.BuscarPorIdAsync(id);
            if (categoria is null)
                throw new KeyNotFoundException("Categoria não encontrada.");

            return MapearParaResposta(categoria);
        }

        public async Task<CategoriaDTO> CriarAsync(CriarCategoriaDTO requisicao)
        {
            if (await _repositorio.NomeExisteAsync(requisicao.Nome))
                throw new InvalidOperationException("Já existe uma categoria com esse nome.");

            var categoria = new Categoria
            {
                Id = Guid.NewGuid(),
                Nome = requisicao.Nome,
                Descricao = requisicao.Descricao,
                Ativa = true,
                DataCriacao = DateTime.UtcNow
            };

            await _repositorio.AdicionarAsync(categoria);
            await _repositorio.SalvarAsync();
            return MapearParaResposta(categoria);
        }

        public async Task<CategoriaDTO> AtualizarAsync(Guid id, CriarCategoriaDTO requisicao)
        {
            var categoria = await _repositorio.BuscarPorIdAsync(id);
            if (categoria is null)
                throw new KeyNotFoundException("Categoria não encontrada.");

            categoria.Nome = requisicao.Nome;
            categoria.Descricao= requisicao.Descricao;
            await _repositorio.AtualizarAsync(categoria);
            await _repositorio.SalvarAsync();
            return MapearParaResposta(categoria);
        }

        public async Task RemoverAsync(Guid id)
        {
            var categoria = await _repositorio.BuscarPorIdAsync(id);
            if (categoria is null)
                throw new KeyNotFoundException("Categoria não encontrada.");
            categoria.Ativa = false;
            await _repositorio.AtualizarAsync(categoria);
            await _repositorio.SalvarAsync();
        }
        private CategoriaDTO MapearParaResposta(Categoria categoria)
            => new()
            {
                Id   = categoria.Id,
                Nome   = categoria.Nome,
                Descricao = categoria.Descricao,
                Ativa = categoria.Ativa,
                DataCriacao = categoria.DataCriacao
            };
    }
}