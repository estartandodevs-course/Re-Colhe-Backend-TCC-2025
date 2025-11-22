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

        // Construtor ORIGINAL (para deserialização JSON)
        public EditarUsuarioCommand()
        {
        }

        // NOVO CONSTRUTOR para receber parâmetros individuais
        public EditarUsuarioCommand(int usuarioId, string? nome, string? email)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
        }

        public bool Validar()
        {
            var validacoes = new InlineValidator<EditarUsuarioCommand>();

            // Valida Nome apenas se fornecido
            validacoes.RuleFor(usuario => usuario.Nome)
                .NotEmpty()
                .When(usuario => usuario.Nome != null)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("Se informado, o nome não pode ser vazio.");

            // Valida Email apenas se fornecido  
            validacoes.RuleFor(usuario => usuario.Email)
                .NotEmpty()
                .When(usuario => usuario.Email != null)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("Se informado, o email não pode ser vazio.")
                .EmailAddress()
                .When(usuario => usuario.Email != null)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("Se informado, o email deve ser válido.");

            // Valida que pelo menos um campo foi fornecido
            validacoes.RuleFor(usuario => usuario)
                .Must(usuario => usuario.Nome != null || usuario.Email != null)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("Pelo menos um campo (nome ou email) deve ser informado para edição.");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }
    }
}