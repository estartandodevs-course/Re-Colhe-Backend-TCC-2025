using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Reclamacoes.ObterPorId
{
    public class ObterReclamacaoPorIdQueryHandler : IRequestHandler<ObterReclamacaoPorIdQuery, CommandResponse<Reclamacao>>
    {
        private readonly IReclamacaoRepository _reclamacaoRepository;

        public ObterReclamacaoPorIdQueryHandler(IReclamacaoRepository reclamacaoRepository)
        {
            _reclamacaoRepository = reclamacaoRepository;
        }

        public async Task<CommandResponse<Reclamacao>> Handle(ObterReclamacaoPorIdQuery request, CancellationToken cancellationToken)
        {
            var reclamacao = await _reclamacaoRepository.BuscarPorIdAsync(request.ReclamacaoId);

            if (reclamacao == null)
            {
                return CommandResponse<Reclamacao>.AdicionarErro("Reclamação não encontrada.", System.Net.HttpStatusCode.NotFound);
            }

            return CommandResponse<Reclamacao>.Sucesso(reclamacao);
        }
    }
}