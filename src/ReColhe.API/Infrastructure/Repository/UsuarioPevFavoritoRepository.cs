using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace ReColhe.API.Infrastructure.Repository
{
    public class UsuarioPevFavoritoRepository : IUsuarioPevFavoritoRepository
    {
        private readonly ApplicationDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UsuarioPevFavoritoRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UsuarioPevFavorito?> BuscarAsync(int usuarioId, int pevId)
        {
            return await _context.UsuarioPevFavoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.PevId == pevId);
        }

        public async Task CriarAsync(UsuarioPevFavorito favorito)
        {
            await _context.UsuarioPevFavoritos.AddAsync(favorito);
        }

        public Task RemoverAsync(UsuarioPevFavorito favorito)
        {
            _context.UsuarioPevFavoritos.Remove(favorito);
            return Task.CompletedTask;
        }
    }
}