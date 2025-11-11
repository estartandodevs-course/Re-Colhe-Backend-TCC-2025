using MediatR;
using ReColhe.Application.Mediator;

namespace ReColhe.Application.Reclamacoes.Excluir
{
    public class ExcluirReclamacaoCommand : IRequest<CommandResponse<Unit>>
    {
        public int ReclamacaoId { get; private set; }

        public ExcluirReclamacaoCommand(int reclamacaoId)
        {
            ReclamacaoId = reclamacaoId;
        }
    }
}