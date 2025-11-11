using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using System.Collections.Generic;

namespace ReColhe.Application.Reclamacoes.Listar
{
    public class ListarReclamacoesQuery : IRequest<CommandResponse<IEnumerable<Reclamacao>>>
    {
    }
}