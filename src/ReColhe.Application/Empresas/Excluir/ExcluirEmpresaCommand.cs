using ReColhe.Application.Mediator;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Net;

namespace ReColhe.Application.Empresas.Excluir
{
    public class ExcluirEmpresaCommand : IRequest<CommandResponse<Unit>>
    {
        public int EmpresaId { get; private set; }

        public ValidationResult? ResultadoDasValidacoes { get; private set; }

        public ExcluirEmpresaCommand(int empresaId)
        {
            EmpresaId = empresaId;
        }

        public bool Validar()
        {
            var validacoes = new InlineValidator<ExcluirEmpresaCommand>();

            validacoes.RuleFor(empresa => empresa.EmpresaId)
                .GreaterThan(0)
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString())
                .WithMessage("ID da empresa é obrigatório.");

            ResultadoDasValidacoes = validacoes.Validate(this);
            return ResultadoDasValidacoes.IsValid;
        }
    }
}