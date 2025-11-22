using ReColhe.Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Net;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ReColhe.Application.Pev.Editar
{
    public class EditarPevCommand : IRequest<CommandResponse<Unit>>
    {
        [JsonIgnore]
        public int PevId { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string HorarioFuncionamento { get; set; } = string.Empty;
        public List<string> Materiais { get; set; } = new List<string>();
        public List<decimal> Posicao { get; set; } = new List<decimal>();
        public ValidationResult? ResultadoDasValidacoes { get; private set; }
        public bool Validar()
        {
            var validacoes = new InlineValidator<EditarPevCommand>();

            validacoes.RuleFor(pev => pev.PevId)
                .GreaterThan(0)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O ID do PEV é inválido.");

            validacoes.RuleFor(pev => pev.Nome)
               .NotEmpty()
               .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .WithMessage("O nome do PEV deve ser informado.");

            validacoes.RuleFor(pev => pev.Posicao)
                .NotNull()
                .Must(p => p.Count == 2)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O campo 'posicao' deve conter exatamente 2 valores (latitude e longitude).");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }

        public void SetPevId(int pevId)
        {
            PevId = pevId;
        }
    }
}