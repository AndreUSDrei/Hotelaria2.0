using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHotelaria.Models;

/// <summary>
/// Reserva normalizada (3FN): FKs para Hóspede, TipoQuarto e Pacote;
/// pagamento e eventos em tabelas próprias.
/// </summary>
public class ReservaEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public decimal ValorTotal { get; set; }
    public string StatusReserva { get; set; } = "Confirmada";

    [ForeignKey(nameof(Hospede))]
    public string HospedeId { get; set; } = string.Empty;
    public virtual HospedeEntity? Hospede { get; set; }

    [ForeignKey(nameof(TipoQuarto))]
    public string TipoQuartoId { get; set; } = string.Empty;
    public virtual TipoQuartoEntity? TipoQuarto { get; set; }

    [ForeignKey(nameof(Pacote))]
    public string? PacoteId { get; set; }
    public virtual PacoteEntity? Pacote { get; set; }

    public virtual PagamentoEntity? Pagamento { get; set; }
    public virtual ICollection<EventoReservaEntity> Eventos { get; set; } = new List<EventoReservaEntity>();
}
