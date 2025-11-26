using ReColhe.Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Net;

namespace ReColhe.Application.Empresas.Criar
{
    public class CriarEmpresaCommand : IRequest<CommandResponse<CriarEmpresaCommandResponse>>
    {
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string? EmailContato { get; set; }
        public string? TelefoneContato { get; set; }

        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public bool Validar()
        {
            var validacoes = new InlineValidator<CriarEmpresaCommand>();

            validacoes.RuleFor(empresa => empresa.NomeFantasia)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O nome fantasia da empresa deve ser informado.")
                .MaximumLength(255)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O nome fantasia deve ter no máximo 255 caracteres.");

            validacoes.RuleFor(empresa => empresa.Cnpj)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O CNPJ deve ser informado.")
                .Must(ValidarCnpj)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O CNPJ informado não é válido.");

            validacoes.RuleFor(empresa => empresa.EmailContato)
                .EmailAddress()
                .When(empresa => !string.IsNullOrEmpty(empresa.EmailContato))
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O email de contato informado não é válido.");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }

        private bool ValidarCnpj(string cnpj)
        {
            // Remove caracteres não numéricos
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                return false;

            // Validação básica de CNPJ (pode ser aprimorada)
            return true;
        }
    }
}