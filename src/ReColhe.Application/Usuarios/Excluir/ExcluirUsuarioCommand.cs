using ReColhe.Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Net;

namespace ReColhe.Application.Usuarios.Excluir
{
    public class ExcluirUsuarioCommand : IRequest<CommandResponse<Unit>>
    {
        public int UsuarioId { get; private set; }

        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public ExcluirUsuarioCommand(int usuarioId)
        {
            UsuarioId = usuarioId;
        }

        public bool Validar()
        {
            var validacoes = new InlineValidator<ExcluirUsuarioCommand>();

            validacoes.RuleFor(usuario => usuario.UsuarioId)
                .GreaterThan(0)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("ID do usuário é obrigatório.");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }
    }
}