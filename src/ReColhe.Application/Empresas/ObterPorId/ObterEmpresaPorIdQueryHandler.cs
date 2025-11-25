using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Empresas.ObterPorId
{
    public class ObterEmpresaPorIdQueryHandler : IRequestHandler<ObterEmpresaPorIdQuery, CommandResponse<Empresa>>
    {
        private readonly IEmpresaRepository _empresaRepository;

        public ObterEmpresaPorIdQueryHandler(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        public async Task<CommandResponse<Empresa>> Handle(ObterEmpresaPorIdQuery request, CancellationToken cancellationToken)
        {
            var empresa = await _empresaRepository.BuscarPorIdAsync(request.EmpresaId);

            if (empresa == null)
            {
                return CommandResponse<Empresa>.AdicionarErro("Empresa não encontrada.", HttpStatusCode.NotFound);
            }

            return CommandResponse<Empresa>.Sucesso(empresa);
        }
    }
}