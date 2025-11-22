using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository
{
    public interface IPevRepository
    {
        Task<IEnumerable<Pev>> GetAllAsync();
        Task<Pev?> GetByIdAsync(int pevId);
        Task AddAsync(Pev pev);
        Task UpdateAsync(Pev pev);
        Task DeleteAsync(Pev pev); 

        Task<bool> PevExistsAsync(int pevId);

        IUnitOfWork UnitOfWork { get; }
    }
}