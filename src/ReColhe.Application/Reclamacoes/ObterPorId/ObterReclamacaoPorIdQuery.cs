using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Entidades;

namespace ReColhe.Application.Reclamacoes.ObterPorId
{
    public class ObterReclamacaoPorIdQuery : IRequest<CommandResponse<Reclamacao>>
    {
        public int ReclamacaoId { get; private set; }

        public ObterReclamacaoPorIdQuery(int reclamacaoId)
        {
            ReclamacaoId = reclamacaoId;
        }
    }
}