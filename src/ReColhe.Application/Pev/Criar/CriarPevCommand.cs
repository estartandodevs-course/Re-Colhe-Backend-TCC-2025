using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ReColhe.Application.Mediator;
using System.Net;

namespace ReColhe.Application.Pev.Criar
{
    public class CriarPevCommand : IRequest<CommandResponse<CriarPevCommandResponse>>
    {
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string HorarioFuncionamento { get; set; } = string.Empty;
        public string Materiais { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public bool Validar()
        {
            var validacoes = new InlineValidator<CriarPevCommand>();

            validacoes.RuleFor(pev => pev.Nome)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O nome do PEV deve ser informado.");

            validacoes.RuleFor(pev => pev.Endereco)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O endereço deve ser informado.");

            validacoes.RuleFor(pev => pev.Latitude)
                .InclusiveBetween(-90, 90)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("Latitude inválida.");

            validacoes.RuleFor(pev => pev.Longitude)
                .InclusiveBetween(-180, 180)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("Longitude inválida.");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }
    }
}