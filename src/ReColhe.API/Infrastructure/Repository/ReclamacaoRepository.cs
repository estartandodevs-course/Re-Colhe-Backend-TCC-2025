using ReColhe.API.Infrastructure;
using ReColhe.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
namespace ReColhe.Domain.Repository
{
    public class ReclamacaoRepository : IReclamacaoRepository
    {
        private readonly ApplicationDbContext _context;

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
            await _context.SaveChangesAsync();
        }

        public async Task EditarAsync(Reclamacao reclamacao)
        {
            _context.Reclamacoes.Update(reclamacao);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var reclamacao = await _context.Reclamacoes.FindAsync(id);
            if (reclamacao != null)
            {
                _context.Reclamacoes.Remove(reclamacao);
                await _context.SaveChangesAsync();
            }
        }
    }
}
