using ReColhe.API.Infrastructure;
using ReColhe.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReColhe.API.Infrastructure.Repository
{
    public class ReclamacaoRepository : IReclamacaoRepository
    {
        private readonly ApplicationDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ReclamacaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reclamacao>> ListarAsync()
        {
            return await _context.Reclamacoes.ToListAsync();
        }

        public async Task<Reclamacao> BuscarPorIdAsync(int id)
        {
            return await _context.Reclamacoes.FindAsync(id);
        }

        public async Task CriarAsync(Reclamacao reclamacao)
        {
            _context.Reclamacoes.Add(reclamacao);
        }

        public async Task EditarAsync(Reclamacao reclamacao)
        {
            _context.Reclamacoes.Update(reclamacao);
        }

        public async Task ExcluirAsync(Reclamacao reclamacao)
        {
            _context.Reclamacoes.Remove(reclamacao);
        }
    }
}