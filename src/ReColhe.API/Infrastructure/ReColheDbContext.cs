using Microsoft.EntityFrameworkCore;
using ReColhe.Domain.Entidades;

namespace ReColhe.API.Infrastructure;

public class ReColheDbContext : DbContext
{
    public ReColheDbContext(DbContextOptions<ReColheDbContext> options) : base(options)
    {
    }

    public DbSet<OrderEntity> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired().HasMaxLength(450);
            entity.Property(e => e.CustomerId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.OrderDate).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            
            // Configurar Items como JSON (MySQL 5.7+ suporta JSON nativamente)
            entity.Property(e => e.Items)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>())
                .HasColumnType("json")
                .HasColumnName("Items");
        });
    }
}
