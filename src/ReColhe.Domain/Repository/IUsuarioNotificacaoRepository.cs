using ReColhe.Domain;
using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository
{
    public interface IUsuarioNotificacaoRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<UsuarioNotificacao> BuscarPorUsuarioENotificacaoAsync(int usuarioId, int notificacaoId);

        Task AtualizarAsync(UsuarioNotificacao usuarioNotificacao);

    }
}