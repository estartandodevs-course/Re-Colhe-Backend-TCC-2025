using ReColhe.Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Net;

namespace ReColhe.Application.Usuarios.Editar
{
    public class EditarUsuarioCommand : IRequest<CommandResponse<Unit>>
    {
        public int UsuarioId { get; private set; }
        public string? Nome { get; private set; }
        public string? Email { get; private set; }

        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public EditarUsuarioCommand(int usuarioId, string? nome, string? email)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
        }

        public bool Validar()
        {
            var validacoes = new InlineValidator<EditarUsuarioCommand>();

            validacoes.RuleFor(usuario => usuario.Nome)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O nome do usuário deve ser informado.");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }
    }
}