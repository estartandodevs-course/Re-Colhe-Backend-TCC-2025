using ReColhe.Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Net;

namespace ReColhe.Application.Usuarios.Criar
{
    public class CriarUsuarioCommand : IRequest<CommandResponse<CriarUsuarioCommandResponse>>
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public int TipoUsuarioId { get; private set; }
        public int? EmpresaId { get; private set; }

        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public CriarUsuarioCommand(string nome, string email, string senha, int tipoUsuarioId, int? empresaId)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            TipoUsuarioId = tipoUsuarioId;
            EmpresaId = empresaId;
        }

        public bool Validar()
        {
            var validacoes = new InlineValidator<CriarUsuarioCommand>();

            validacoes.RuleFor(usuario => usuario.Nome)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O nome do usuário deve ser informado.");

            validacoes.RuleFor(usuario => usuario.Email)
               .NotEmpty()
               .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .WithMessage("O email deve ser informado.")
               .EmailAddress()
               .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .WithMessage("O email informado não é válido.");

            validacoes.RuleFor(usuario => usuario.Senha)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("A senha deve ser informada.")
                .MinimumLength(6)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("A senha deve ter pelo menos 6 caracteres.");

            validacoes.RuleFor(usuario => usuario.TipoUsuarioId)
                .InclusiveBetween(1, 2)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O tipo de usuário deve ser 1 (Comum) ou 2 (Colaborador).");

            validacoes.RuleFor(usuario => usuario.EmpresaId)
                .NotNull()
                .When(usuario => usuario.TipoUsuarioId == 2)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O campo 'empresa_id' é obrigatório para usuários Colaboradores.");

            validacoes.RuleFor(usuario => usuario.EmpresaId)
                .Null()
                .When(usuario => usuario.TipoUsuarioId == 1)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("Usuários comuns não devem ter empresa vinculada.");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }
    }
}