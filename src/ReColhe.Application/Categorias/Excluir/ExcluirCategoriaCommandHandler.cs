using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Categorias.Excluir
{
    public class ExcluirCategoriaCommandHandler : IRequestHandler<ExcluirCategoriaCommand, CommandResponse<Unit>>
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ExcluirCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(ExcluirCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);

            if (categoria == null)
            {
                return CommandResponse<Unit>.AdicionarErro("Categoria não encontrada.", System.Net.HttpStatusCode.NotFound);
            }

            await _categoriaRepository.RemoverAsync(categoria);

            // Regra 2: Handler é quem salva!
            await _categoriaRepository.UnitOfWork.CommitAsync(cancellationToken);

            return CommandResponse<Unit>.Sucesso(Unit.Value, System.Net.HttpStatusCode.NoContent);
        }
    }
}