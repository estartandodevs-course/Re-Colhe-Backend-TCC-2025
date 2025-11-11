using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;

namespace ReColhe.Application.Categorias.ObterPorId
{
    public class ObterCategoriaPorIdQuery : IRequest<CommandResponse<Categoria>>
    {
        public int CategoriaId { get; private set; }

        public ObterCategoriaPorIdQuery(int categoriaId)
        {
            CategoriaId = categoriaId;
        }
    }
}