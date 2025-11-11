using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository
{
    public interface ICategoriaRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<Categoria>> ListarAsync();
        Task<Categoria> BuscarPorIdAsync(int categoriaId);
        Task CriarAsync(Categoria categoria);
        Task AtualizarAsync(Categoria categoria);
        Task RemoverAsync(Categoria categoria);
        Task<Categoria> ObterPorIdAsync(int id);
    }
}