using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Empresas.Listar
{
    public class ListarEmpresasQueryHandler : IRequestHandler<ListarEmpresasQuery, CommandResponse<ListarEmpresasResponse>>
    {
        private readonly IEmpresaRepository _empresaRepository;

        public ListarEmpresasQueryHandler(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        public async Task<CommandResponse<ListarEmpresasResponse>> Handle(ListarEmpresasQuery request, CancellationToken cancellationToken)
        {
            var empresas = await _empresaRepository.ListarAsync();

            var response = new ListarEmpresasResponse
            {
                Empresas = empresas
            };

            return CommandResponse<ListarEmpresasResponse>.Sucesso(response);
        }
    }
}