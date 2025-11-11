using MediatR;
using ReColhe.Domain.Entidades;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Reclamacoes.Apoiar
{
    public class ApoiarReclamacaoCommandHandler : IRequestHandler<ApoiarReclamacaoCommand, CommandResponse<Unit>>
    {
        private readonly IApoioReclamacaoRepository _repository;
        public ApoiarReclamacaoCommandHandler(IApoioReclamacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Unit>> Handle(ApoiarReclamacaoCommand command, CancellationToken cancellationToken)
        {
            var apoio = new ApoioReclamacao
            {
                UsuarioId = command.UsuarioId,
                ReclamacaoId = command.ReclamacaoId,
                DataApoio = DateTime.Now
            };

            await _repository.CriarAsync(apoio);

            await _repository.UnitOfWork.CommitAsync(cancellationToken);

            return CommandResponse<Unit>.Sucesso(Unit.Value, System.Net.HttpStatusCode.OK);
        }
    }
}