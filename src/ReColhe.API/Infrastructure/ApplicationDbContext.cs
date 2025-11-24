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
    public DbSet<Reclamacao> Reclamacoes { get; set; }
    public DbSet<ApoioReclamacao> ApoioReclamacoes { get; set; }
    public DbSet<Notificacao> Notificacoes { get; set; }
    public DbSet<UsuarioNotificacao> UsuarioNotificacoes { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Empresa>()
            .HasIndex(e => e.Cnpj)
            .IsUnique();
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
        modelBuilder.Entity<ApoioReclamacao>()
            .HasKey(ar => new { ar.UsuarioId, ar.ReclamacaoId });
        modelBuilder.Entity<TipoUsuario>().HasData(
            new TipoUsuario { TipoUsuarioId = 1, Nome = "Comum" },
            new TipoUsuario { TipoUsuarioId = 2, Nome = "Colaborador" }
        );
        modelBuilder.Entity<Empresa>().HasData(
             new Empresa
             {
                 EmpresaId = 1,
                 NomeFantasia = "Empresa Teste",
                 Cnpj = "00.000.000/0001-00",
                 EmailContato = "contato@empresa.com",
                 TelefoneContato = "99999-9999"
             }
         );
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}