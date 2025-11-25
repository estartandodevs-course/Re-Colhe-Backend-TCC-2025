using MediatR;
using ReColhe.Application.Mediator;
using System.Collections.Generic;

namespace ReColhe.Application.Empresas.Listar
{
    public class ListarEmpresasQuery : IRequest<CommandResponse<ListarEmpresasResponse>>
    {
    }
}