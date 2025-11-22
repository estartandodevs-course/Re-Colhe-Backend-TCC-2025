using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Categorias.Listar
{
    public class ListarCategoriasQueryHandler : IRequestHandler<ListarCategoriasQuery, CommandResponse<IEnumerable<Categoria>>>
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ListarCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<CommandResponse<IEnumerable<Categoria>>> Handle(ListarCategoriasQuery request, CancellationToken cancellationToken)
        {
            var categorias = await _categoriaRepository.ListarAsync();

            return CommandResponse<IEnumerable<Categoria>>.Sucesso(categorias);
        }
    }
}