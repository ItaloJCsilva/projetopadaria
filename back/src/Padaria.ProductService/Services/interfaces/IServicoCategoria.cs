using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Padaria.ProductService.DTOs;

namespace Padaria.ProductService.Services.interfaces
{
    public interface IServicoCategoria
    {
        Task<List<CategoriaDTO>> ListarTodasAsync();
        Task<CategoriaDTO> BuscarPorIdAsync(Guid id);
        Task<CategoriaDTO> CriarAsync(CriarCategoriaDTO requisicao);
        Task<CategoriaDTO> AtualizarAsync(Guid id, CriarCategoriaDTO requisicao);
        Task RemoverAsync(Guid id);
    }
}