using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;

namespace ReColhe.Domain.Repository
{
    public interface IApplicationDbContext
    {
        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Empresa> Empresas { get; set; }
        DbSet<TipoUsuario> TiposUsuario { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}