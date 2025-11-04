using Microsoft.EntityFrameworkCore;

namespace ReColhe.API.Infrastructure;

public class ReColheDbContext : DbContext
{
    public ReColheDbContext(DbContextOptions<ReColheDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Modelo simplificado - sem entidades específicas por enquanto
    }
}
