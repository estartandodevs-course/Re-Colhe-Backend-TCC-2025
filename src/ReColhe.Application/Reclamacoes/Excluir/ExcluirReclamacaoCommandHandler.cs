using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Reclamacoes.Excluir
{
    public class ExcluirReclamacaoCommandHandler : IRequestHandler<ExcluirReclamacaoCommand, CommandResponse<Unit>>
    {
        private readonly IReclamacaoRepository _reclamacaoRepository;

        public ExcluirReclamacaoCommandHandler(IReclamacaoRepository reclamacaoRepository)
        {
            _reclamacaoRepository = reclamacaoRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(ExcluirReclamacaoCommand request, CancellationToken cancellationToken)
        {
            var reclamacao = await _reclamacaoRepository.BuscarPorIdAsync(request.ReclamacaoId);

            if (reclamacao == null)
            {
                return CommandResponse<Unit>.AdicionarErro("Reclamação não encontrada.", System.Net.HttpStatusCode.NotFound);
            }

            await _reclamacaoRepository.ExcluirAsync(reclamacao);
            await _reclamacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

            return CommandResponse<Unit>.Sucesso(Unit.Value, System.Net.HttpStatusCode.NoContent);
        }
    }
}