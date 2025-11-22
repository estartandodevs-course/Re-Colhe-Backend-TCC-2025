using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReColhe.API.Infrastructure.Repository
{
    public class PevRepository : IPevRepository
    {
        private readonly ApplicationDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public PevRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Pev>> GetAllAsync()
        {
            return await _context.Pevs.AsNoTracking().ToListAsync();
        }

        public async Task<Pev?> GetByIdAsync(int pevId)
        {
            return await _context.Pevs.FindAsync(pevId);
        }

        public async Task AddAsync(Pev pev)
        {
            await _context.Pevs.AddAsync(pev);
        }

        public Task UpdateAsync(Pev pev)
        {
            _context.Pevs.Update(pev);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Pev pev)
        {
            _context.Pevs.Remove(pev);
            return Task.CompletedTask;
        }

        public async Task<bool> PevExistsAsync(int pevId)
        {
            return await _context.Pevs.AnyAsync(e => e.PevId == pevId);
        }
    }
}