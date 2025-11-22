using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using ReColhe.API.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReColhe.API.Infrastructure.Repository
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        private readonly ApplicationDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public NotificacaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notificacao>> ListarPorUsuarioAsync(int usuarioId)
        {
            return await _context.Notificacoes
                .Where(n => n.UsuarioNotificacoes.Any(un => un.UsuarioId == usuarioId))
                .ToListAsync();
        }

        public async Task<Notificacao> BuscarPorIdAsync(int notificacaoId)
        {
            return await _context.Notificacoes.FindAsync(notificacaoId);
        }

        public async Task CriarAsync(Notificacao notificacao)
        {
            _context.Notificacoes.Add(notificacao);
            
        }

        public async Task AtualizarAsync(Notificacao notificacao)
        {
            _context.Notificacoes.Update(notificacao);
        }

        public async Task RemoverAsync(Notificacao notificacao)
        {
            _context.Notificacoes.Remove(notificacao);
        }
    }
}