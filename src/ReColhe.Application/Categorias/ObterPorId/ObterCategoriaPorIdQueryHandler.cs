using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Categorias.ObterPorId
{
    public class ObterCategoriaPorIdQueryHandler : IRequestHandler<ObterCategoriaPorIdQuery, CommandResponse<Categoria>>
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ObterCategoriaPorIdQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<CommandResponse<Categoria>> Handle(ObterCategoriaPorIdQuery request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.BuscarPorIdAsync(request.CategoriaId);

            if (categoria == null)
            {
                return CommandResponse<Categoria>.AdicionarErro("Categoria não encontrada.", System.Net.HttpStatusCode.NotFound);
            }

            return CommandResponse<Categoria>.Sucesso(categoria);
        }
    }
}