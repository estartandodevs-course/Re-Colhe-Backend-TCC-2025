using ReColhe.Domain;
using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository
{
    public interface IReclamacaoRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<Reclamacao>> ListarAsync();
        Task<Reclamacao> BuscarPorIdAsync(int id);
        Task CriarAsync(Reclamacao reclamacao);
        Task EditarAsync(Reclamacao reclamacao);

        Task ExcluirAsync(Reclamacao reclamacao);
    }
}