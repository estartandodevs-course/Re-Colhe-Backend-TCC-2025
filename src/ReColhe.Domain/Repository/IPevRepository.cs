using ReColhe.Domain.Entidades; // Importa as entidades
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReColhe.Domain.Repository
{
    /// <summary>
    /// Contrato (Interface) para o repositório de PEVs.
    /// Define as operações de banco de dados necessárias para a entidade PEV.
    /// </summary>
    public interface IPevRepository
    {
        // CRUD Básico
        Task<IEnumerable<Pev>> GetAllAsync();
        Task<Pev> GetByIdAsync(int pevId);
        Task AddAsync(Pev pev);
        Task UpdateAsync(Pev pev);
        Task DeleteAsync(int pevId);

        // Rotas do
        //Task<IEnumerable<Pev>> GetFavoritosByUsuarioIdAsync(int usuarioId);
        //Task AddFavoritoAsync(UsuarioPevFavorito favorito);
        //Task RemoveFavoritoAsync(UsuarioPevFavorito favorito);

        //// Métodos auxiliares
        //Task<UsuarioPevFavorito> GetFavoritoAsync(int usuarioId, int pevId);
        //Task<bool> PevExistsAsync(int pevId);
        Task<bool> SaveChangesAsync();
    }
}