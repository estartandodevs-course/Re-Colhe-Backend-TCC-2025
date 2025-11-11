using MediatR;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Categorias.Excluir
{
    public class ExcluirCategoriaCommand : IRequest<CommandResponse<Unit>>
    {
        public int CategoriaId { get; private set; }

        public ExcluirCategoriaCommand(int categoriaId)
        {
            CategoriaId = categoriaId;
        }
    }
}