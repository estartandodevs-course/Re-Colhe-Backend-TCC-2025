using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;
using ReColhe.Domain.Repository;

namespace ReColhe.API.Infrastructure;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<TipoUsuario> TiposUsuario { get; set; }
    public DbSet<UsuarioPevFavorito> UsuarioPevFavoritos { get; set; }
    public DbSet<Pev> Pevs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UsuarioPevFavorito>()
            .HasKey(upf => new { upf.UsuarioId, upf.PevId });
        modelBuilder.Entity<UsuarioPevFavorito>()
            .HasOne(upf => upf.Usuario)
            .WithMany(u => u.PevFavoritos)
            .HasForeignKey(upf => upf.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UsuarioPevFavorito>()
            .HasOne(upf => upf.Pev)
            .WithMany(p => p.UsuariosFavoritos)
            .HasForeignKey(upf => upf.PevId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}