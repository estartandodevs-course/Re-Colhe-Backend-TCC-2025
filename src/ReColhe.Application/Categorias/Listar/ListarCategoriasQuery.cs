using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using System.Collections.Generic;

namespace ReColhe.Application.Categorias.Listar
{
    public class ListarCategoriasQuery : IRequest<CommandResponse<IEnumerable<Categoria>>>
    {
    }
}