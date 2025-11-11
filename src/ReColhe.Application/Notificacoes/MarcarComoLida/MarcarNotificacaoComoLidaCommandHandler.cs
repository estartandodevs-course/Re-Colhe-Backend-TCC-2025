using MediatR;
using ReColhe.Application.Mediator;
using ReColhe.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Application.Notificacoes.MarcarComoLida
{
    public class MarcarNotificacaoComoLidaCommandHandler : IRequestHandler<MarcarNotificacaoComoLidaCommand, CommandResponse<Unit>>
    {
        private readonly IUsuarioNotificacaoRepository _usuarioNotificacaoRepository;

        public MarcarNotificacaoComoLidaCommandHandler(IUsuarioNotificacaoRepository usuarioNotificacaoRepository)
        {
            _usuarioNotificacaoRepository = usuarioNotificacaoRepository;
        }

        public async Task<CommandResponse<Unit>> Handle(MarcarNotificacaoComoLidaCommand request, CancellationToken cancellationToken)
        {
            var link = await _usuarioNotificacaoRepository.BuscarPorUsuarioENotificacaoAsync(request.UsuarioId, request.NotificacaoId);

            if (link == null)
            {
                return CommandResponse<Unit>.AdicionarErro("Notificação de usuário não encontrada.", System.Net.HttpStatusCode.NotFound);
            }

            link.Lida = true;

            await _usuarioNotificacaoRepository.AtualizarAsync(link);

            await _usuarioNotificacaoRepository.UnitOfWork.CommitAsync(cancellationToken);

            return CommandResponse<Unit>.Sucesso(Unit.Value, System.Net.HttpStatusCode.NoContent);
        }
    }
}