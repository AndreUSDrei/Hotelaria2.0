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
    public DbSet<PagamentoEntity> Pagamentos { get; set; } = null!;
    public DbSet<EventoReservaEntity> EventosReserva { get; set; } = null!;
    public DbSet<AcaoEventoEntity> AcoesEvento { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PacoteEntity>()
            .HasMany(p => p.Refeicoes)
            .WithMany(r => r.Pacotes)
            .UsingEntity(j => j.ToTable("PacotesRefeicoes"));

        modelBuilder.Entity<PacoteEntity>()
            .HasMany(p => p.ServicosAdicionais)
            .WithMany(s => s.Pacotes)
            .UsingEntity(j => j.ToTable("PacotesServicosAdicionais"));

        // 1:1 Pagamento ↔ Reserva
        modelBuilder.Entity<ReservaEntity>()
            .HasOne(r => r.Pagamento)
            .WithOne(p => p.Reserva)
            .HasForeignKey<PagamentoEntity>(p => p.ReservaId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:N Eventos ↔ Reserva
        modelBuilder.Entity<ReservaEntity>()
            .HasMany(r => r.Eventos)
            .WithOne(e => e.Reserva)
            .HasForeignKey(e => e.ReservaId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:N Ações ↔ Evento
        modelBuilder.Entity<EventoReservaEntity>()
            .HasMany(e => e.Acoes)
            .WithOne(a => a.Evento)
            .HasForeignKey(a => a.EventoReservaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Evita nomes duplicados de tipo de quarto / hóspede (integridade)
        modelBuilder.Entity<TipoQuartoEntity>()
            .HasIndex(t => t.Nome)
            .IsUnique();

        modelBuilder.Entity<HospedeEntity>()
            .HasIndex(h => h.Nome);
    }
}
