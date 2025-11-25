using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;

namespace ReColhe.Application.Empresas.ObterPorId
{
    public class ObterEmpresaPorIdQuery : IRequest<CommandResponse<Empresa>>
    {
        public int EmpresaId { get; private set; }

        public ObterEmpresaPorIdQuery(int empresaId)
        {
            EmpresaId = empresaId;
        }
    }
}