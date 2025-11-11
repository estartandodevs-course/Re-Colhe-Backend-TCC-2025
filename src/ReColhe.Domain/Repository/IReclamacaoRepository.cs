using ReColhe.Domain;
using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository
{
    public interface IApoioReclamacaoRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<ApoioReclamacao>> ListarPorReclamacaoAsync(int reclamacaoId);
        Task<ApoioReclamacao> BuscarAsync(int usuarioId, int reclamacaoId);
        Task CriarAsync(ApoioReclamacao apoio);

        Task RemoverAsync(ApoioReclamacao apoio);
    }
}