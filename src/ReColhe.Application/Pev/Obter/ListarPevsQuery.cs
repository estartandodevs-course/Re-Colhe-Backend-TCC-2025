using MediatR;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Pev.Obter
{
    // Query vazia apenas solicita a lista retorna uma lista na resposta.
    public class ListarPevsQuery : IRequest<CommandResponse<ListarPevsResponse>>
    {
    }
}