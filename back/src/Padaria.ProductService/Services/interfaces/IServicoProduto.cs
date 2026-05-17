using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.ProductService.DTOs;

namespace Padaria.ProductService.Services.interfaces
{
    public interface IServicoProduto
    {
        Task<List<ProdutoDTO>> ListarTodosAsync();
        Task<List<ProdutoDTO>> ListarPorCategoriaAsync(Guid categoriaId);
        Task<ProdutoDTO> BuscarPorIdAsync(Guid id);
        Task<ProdutoDTO> CriarAsync(CriarProdutoDTO requisicao);
        Task<ProdutoDTO> AtualizarAsync(Guid id, AtualizarProdutoDTO requisicao);
        Task RemoverAsync(Guid id);
    }
}