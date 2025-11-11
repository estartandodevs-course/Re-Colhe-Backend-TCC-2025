using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using ReColhe.API.Infrastructure;

public class NotificacaoRepository : INotificacaoRepository
{
    private readonly ApplicationDbContext _context;

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
        await _context.SaveChangesAsync();
    }

    public async Task MarcarComoLidaAsync(int usuarioId, int notificacaoId)
    {
        var usuarioNotificacao = await _context.UsuarioNotificacoes
            .FirstOrDefaultAsync(un => un.UsuarioId == usuarioId && un.NotificacaoId == notificacaoId);

        if (usuarioNotificacao != null)
        {
            usuarioNotificacao.Lida = true;
            await _context.SaveChangesAsync();
        }
    }
}
