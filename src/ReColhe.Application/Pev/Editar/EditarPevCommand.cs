using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ReColhe.Application.Mediator;
using System.Net;
using System.Text.Json.Serialization;

namespace ReColhe.Application.Pev.Editar
{
    public class EditarPevCommand : IRequest<CommandResponse<Unit>>
    {
        [JsonIgnore]
        public int PevId { get; set; }
        public string? Nome { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
        public string? OpeningDays { get; set; }

        public List<string>? Materiais { get; set; }
        public List<decimal>? Posicao { get; set; }

        [JsonIgnore]
        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public bool Validar()
        {
            var validacoes = new InlineValidator<EditarPevCommand>();
            var materiaisValidos = new List<string> { "metal", "papel", "vidro", "plastico" };

            validacoes.RuleFor(pev => pev.PevId)
                .GreaterThan(0)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("O ID do PEV é inválido.");

            // Validação para campos que, se enviados (não nulos), não podem ser vazios.
            validacoes.RuleFor(pev => pev.Nome)
               .NotEmpty()
               .When(pev => pev.Nome != null)
               .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .WithMessage("O nome do PEV deve ser informado.");

            validacoes.RuleFor(pev => pev.OpenTime)
               .NotEmpty()
               .When(pev => pev.OpenTime != null)
               .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .WithMessage("O horário de abertura (openTime) não pode ser vazio.");

            validacoes.RuleFor(pev => pev.OpeningDays)
               .NotEmpty()
               .When(pev => pev.OpeningDays != null)
               .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
               .WithMessage("Os dias de funcionamento (openingDays) não podem ser vazios.");

            // Validação dos Materiais: Se o campo for enviado, ele deve ser válido
            validacoes.RuleFor(pev => pev.Materiais)
                .Must(m => m!.Any())
                .When(pev => pev.Materiais != null)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("A lista de materiais deve ser informada.")
                .Must(m => m!.All(material => materiaisValidos.Contains(material.ToLower())))
                .When(pev => pev.Materiais != null)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("A lista de materiais contém itens inválidos. Somente são aceitos: metal, papel, vidro e plastico.");

            validacoes.RuleFor(pev => pev.Posicao)
                .Must(p => p!.Count == 2)
                .When(pev => pev.Posicao != null)
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