using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using ReColhe.API.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReColhe.API.Infrastructure.Repository
{
    public class ApoioReclamacaoRepository : IApoioReclamacaoRepository
    {
        private readonly ApplicationDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ApoioReclamacaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApoioReclamacao>> ListarPorReclamacaoAsync(int reclamacaoId)
        {
            return await _context.ApoioReclamacoes
                .Where(a => a.ReclamacaoId == reclamacaoId)
                .ToListAsync();
        }

        public async Task<ApoioReclamacao> BuscarAsync(int usuarioId, int reclamacaoId)
        {
            return await _context.ApoioReclamacoes
                .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId && a.ReclamacaoId == reclamacaoId);
        }

        public async Task CriarAsync(ApoioReclamacao apoio)
        {
            _context.ApoioReclamacoes.Add(apoio);
        }

        public async Task RemoverAsync(ApoioReclamacao apoio)
        {
            _context.ApoioReclamacoes.Remove(apoio);
        }
    }
}