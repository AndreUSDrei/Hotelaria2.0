using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHotelaria.Models;

/// <summary>
/// Evento de mudança de status da reserva (substitui JSON EventosReserva — atende 1FN/3FN).
/// </summary>
public class EventoReservaEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Status { get; set; } = string.Empty;
    public DateTime Quando { get; set; }

    [ForeignKey(nameof(Reserva))]
    public string ReservaId { get; set; } = string.Empty;
    public virtual ReservaEntity? Reserva { get; set; }

    public virtual ICollection<AcaoEventoEntity> Acoes { get; set; } = new List<AcaoEventoEntity>();
}
