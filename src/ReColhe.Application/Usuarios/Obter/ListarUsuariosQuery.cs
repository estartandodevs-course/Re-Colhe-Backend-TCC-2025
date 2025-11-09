using MediatR;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Usuarios.Obter
{
    public record ListarUsuariosQuery : IRequest<CommandResponse<ListarUsuariosResponse>>;
}