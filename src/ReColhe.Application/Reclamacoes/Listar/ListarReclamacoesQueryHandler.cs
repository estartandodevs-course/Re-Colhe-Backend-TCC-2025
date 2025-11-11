using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Reclamacoes.Listar
{
    
    public class ListarReclamacoesQueryHandler : IRequestHandler<ListarReclamacoesQuery, CommandResponse<IEnumerable<Reclamacao>>>
    {
        private readonly IReclamacaoRepository _reclamacaoRepository;

        public ListarReclamacoesQueryHandler(IReclamacaoRepository reclamacaoRepository)
        {
            _reclamacaoRepository = reclamacaoRepository;
        }

        public async Task<CommandResponse<IEnumerable<Reclamacao>>> Handle(ListarReclamacoesQuery request, CancellationToken cancellationToken)
        {
            var reclamacoes = await _reclamacaoRepository.ListarAsync();

            return CommandResponse<IEnumerable<Reclamacao>>.Sucesso(reclamacoes);
        }
    }
}