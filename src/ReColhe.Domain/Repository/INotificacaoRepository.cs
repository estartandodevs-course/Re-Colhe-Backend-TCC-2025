using ReColhe.Domain;
using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository
{
    public interface INotificacaoRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<Notificacao>> ListarPorUsuarioAsync(int usuarioId);
        Task<Notificacao> BuscarPorIdAsync(int notificacaoId);
        Task CriarAsync(Notificacao notificacao);

        Task AtualizarAsync(Notificacao notificacao);

        Task RemoverAsync(Notificacao notificacao);
    }
}