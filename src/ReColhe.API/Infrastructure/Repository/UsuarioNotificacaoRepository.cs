using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using ReColhe.API.Infrastructure;
using System.Threading.Tasks;

namespace ReColhe.API.Infrastructure.Repository
{
    public class UsuarioNotificacaoRepository : IUsuarioNotificacaoRepository
    {
        private readonly ApplicationDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UsuarioNotificacaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioNotificacao> BuscarPorUsuarioENotificacaoAsync(int usuarioId, int notificacaoId)
        {
            return await _context.UsuarioNotificacoes.FirstOrDefaultAsync(un => un.UsuarioId == usuarioId && un.NotificacaoId == notificacaoId);
        }

        public async Task AtualizarAsync(UsuarioNotificacao usuarioNotificacao)
        {
            _context.UsuarioNotificacoes.Update(usuarioNotificacao);
        }
    }
}