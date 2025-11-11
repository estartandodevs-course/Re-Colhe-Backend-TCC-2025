using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;

namespace ReColhe.API.Infrastructure.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .Include(u => u.Empresa)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailJaUtilizado(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> EmpresaExiste(int empresaId)
        {
            return await _context.Empresas.AnyAsync(e => e.EmpresaId == empresaId);
        }

        public async Task Adicionar(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }

        public Task Excluir(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            return Task.CompletedTask;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.CommitAsync(cancellationToken);
        }


        public async Task<UsuarioPevFavorito?> ObterFavoritoAsync(int usuarioId, int pevId)
        {
            return await _context.UsuarioPevFavoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.PevId == pevId);
        }

        public async Task AdicionarFavorito(UsuarioPevFavorito favorito)
        {
            await _context.UsuarioPevFavoritos.AddAsync(favorito);
        }

        public Task RemoverFavorito(UsuarioPevFavorito favorito)
        {
            _context.UsuarioPevFavoritos.Remove(favorito);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<Usuario>> ObterTodosComTipoUsuarioAsync()
        {
            return await _context.Usuarios
                .Include(u => u.TipoUsuario)  // Inclui o TipoUsuario
                .AsNoTracking()               // Apenas leitura - melhor performance
                .ToListAsync();
        }

    }
}