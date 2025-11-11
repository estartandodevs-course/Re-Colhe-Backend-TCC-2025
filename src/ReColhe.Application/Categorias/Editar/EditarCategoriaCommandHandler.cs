using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Categorias.Editar
{
    public class EditarCategoriaCommandHandler : IRequestHandler<EditarCategoriaCommand, CommandResponse<Unit>>
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public EditarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(EditarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);

            if (categoria == null)
            {
                return CommandResponse<Unit>.AdicionarErro("Categoria não encontrada.", System.Net.HttpStatusCode.NotFound);
            }

            categoria.Nome = request.Nome;
        

            await _categoriaRepository.AtualizarAsync(categoria);

            await _categoriaRepository.UnitOfWork.CommitAsync(cancellationToken);

            return CommandResponse<Unit>.Sucesso(Unit.Value, System.Net.HttpStatusCode.NoContent);
        }
    }
}