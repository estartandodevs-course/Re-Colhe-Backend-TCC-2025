using ReColhe.Application.Mediator;
using MediatR;

namespace ReColhe.Application.Auth.Login
{
    public class LoginCommand : IRequest<CommandResponse<LoginCommandResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}