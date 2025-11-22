using MediatR;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Usuarios.Obter
{
    public record ObterUsuarioPorIdQuery(int UsuarioId) : IRequest<CommandResponse<ObterUsuarioPorIdResponse>>;
}