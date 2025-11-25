using ReColhe.Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Net;
using System.Text.Json.Serialization;

namespace ReColhe.Application.Empresas.Editar
{
    public class EditarEmpresaCommand : IRequest<CommandResponse<Unit>>
    {
        [JsonIgnore]
        public int EmpresaId { get; set; }

        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string? EmailContato { get; set; }
        public string? TelefoneContato { get; set; }

        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public void SetEmpresaId(int empresaId) => EmpresaId = empresaId;

        public bool Validar()
        {
            var validacoes = new InlineValidator<EditarEmpresaCommand>();

            validacoes.RuleFor(empresa => empresa.EmpresaId)
                .GreaterThan(0)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O ID da empresa é inválido.");

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
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());
            return cnpj.Length == 14;
        }
    }
}