using Microsoft.EntityFrameworkCore;
using ReColhe.API.Infrastructure;
using ReColhe.Domain;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System.Threading.Tasks;

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
        return await _context.UsuarioNotificacoes
            .FirstOrDefaultAsync(un => un.UsuarioId == usuarioId && un.NotificacaoId == notificacaoId);
    }

    public async Task AtualizarAsync(UsuarioNotificacao usuarioNotificacao)
    {
        _context.UsuarioNotificacoes.Update(usuarioNotificacao);    
    }
}