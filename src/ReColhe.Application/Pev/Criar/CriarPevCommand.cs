using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ReColhe.Application.Mediator;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.Linq; 

namespace ReColhe.Application.Pev.Criar
{
    public class CriarPevCommand : IRequest<CommandResponse<CriarPevCommandResponse>>
    {
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string OpenTime { get; set; } = string.Empty;
        public string CloseTime { get; set; } = string.Empty;
        public string OpeningDays { get; set; } = string.Empty;

        public List<string> Materiais { get; set; } = new List<string>();
        public List<decimal> Posicao { get; set; } = new List<decimal>();
        [JsonIgnore]
        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public bool Validar()
        {
            var validacoes = new InlineValidator<CriarPevCommand>();
            var materiaisValidos = new List<string> { "metal", "papel", "vidro", "plastico" };

            validacoes.RuleFor(pev => pev.Nome)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O nome do PEV deve ser informado.");

            validacoes.RuleFor(pev => pev.Endereco)
                .NotEmpty()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O endereço deve ser informado.");

            validacoes.RuleFor(pev => pev.OpenTime)
               .NotEmpty()
               .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .WithMessage("O horário de abertura (openTime) deve ser informado.");

            validacoes.RuleFor(pev => pev.OpeningDays)
               .NotEmpty()
               .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .WithMessage("Os dias de funcionamento (openingDays) devem ser informados.");

            // Validação dos Materiais: Restrito aos 4 tipos
            validacoes.RuleFor(pev => pev.Materiais)
                .Must(m => m != null && m.Any())
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("A lista de materiais deve ser informada.")
                .Must(m => m.All(material => materiaisValidos.Contains(material.ToLower())))
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("A lista de materiais contém itens inválidos. Somente são aceitos: metal, papel, vidro e plastico.");

            // Validação da latitude e longitude( DEVE TER 2 VALORES )
            validacoes.RuleFor(pev => pev.Posicao)
                .NotNull()
                .Must(p => p.Count == 2)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O campo 'posicao' deve conter exatamente 2 valores (latitude e longitude).");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }
    }
}