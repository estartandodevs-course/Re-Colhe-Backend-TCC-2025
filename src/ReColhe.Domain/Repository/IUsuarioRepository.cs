using ReColhe.Domain.Entidades;
using System.Threading;
using System.Threading.Tasks;

namespace ReColhe.Domain.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<bool> EmailJaUtilizado(string email);
        Task<bool> EmpresaExiste(int empresaId);
        Task Adicionar(Usuario usuario);
        Task Excluir(Usuario usuario);
        Task<int> CommitAsync(System.Threading.CancellationToken cancellationToken = default);
        Task<IEnumerable<Usuario>> ObterTodosComTipoUsuarioAsync();

        IUnitOfWork UnitOfWork { get; }

    }

    public interface IUnitOfWork
    {
        Task<int> CommitAsync(System.Threading.CancellationToken cancellationToken = default);
    }
}