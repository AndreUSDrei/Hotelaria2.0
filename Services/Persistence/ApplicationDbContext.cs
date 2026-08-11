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
}
