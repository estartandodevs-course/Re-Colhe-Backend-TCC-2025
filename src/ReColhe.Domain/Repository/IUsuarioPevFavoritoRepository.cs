using ReColhe.Domain.Entidades;
using System.Threading.Tasks;

namespace ReColhe.Domain.Repository
{
    public interface IUsuarioPevFavoritoRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Task<UsuarioPevFavorito?> BuscarAsync(int usuarioId, int pevId);
        Task CriarAsync(UsuarioPevFavorito favorito);
        Task RemoverAsync(UsuarioPevFavorito favorito);
    }
}