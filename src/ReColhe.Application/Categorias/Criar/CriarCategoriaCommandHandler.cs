using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Categorias.Criar
{
    public class CriarCategoriaCommandHandler : IRequestHandler<CriarCategoriaCommand, CommandResponse<CriarCategoriaCommandResponse>>
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CriarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<CommandResponse<CriarCategoriaCommandResponse>> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = new Categoria
            {
                Nome = request.Nome,
                
            };

            await _categoriaRepository.CriarAsync(categoria);

            await _categoriaRepository.UnitOfWork.CommitAsync(cancellationToken);

            var response = new CriarCategoriaCommandResponse { CategoriaId = categoria.CategoriaId };

            return CommandResponse<CriarCategoriaCommandResponse>.Sucesso(response, System.Net.HttpStatusCode.Created);
        }
    }
}