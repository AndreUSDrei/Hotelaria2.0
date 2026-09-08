using Microsoft.EntityFrameworkCore;
using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ReservaEntity> Reservas { get; set; } = null!;
    public DbSet<HospedeEntity> Hospedes { get; set; } = null!;
    public DbSet<TipoQuartoEntity> TiposQuarto { get; set; } = null!;

    public DbSet<PacoteEntity> Pacotes { get; set; } = null!;
    public DbSet<RefeicaoEntity> Refeicoes { get; set; } = null!;
    public DbSet<ServicoAdicionalEntity> ServicosAdicionais { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



        // Many-to-many relationship: Pacotes <-> Refeicoes
        modelBuilder.Entity<PacoteEntity>()
            .HasMany(p => p.Refeicoes)
            .WithMany(r => r.Pacotes)
            .UsingEntity(j => j.ToTable("PacotesRefeicoes"));

        // Many-to-many relationship: Pacotes <-> ServicosAdicionais
        modelBuilder.Entity<PacoteEntity>()
            .HasMany(p => p.ServicosAdicionais)
            .WithMany(s => s.Pacotes)
            .UsingEntity(j => j.ToTable("PacotesServicosAdicionais"));
    }
}
