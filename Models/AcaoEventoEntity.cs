using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHotelaria.Models;

/// <summary>
/// Ação notificada em um evento (E-mail, Limpeza, Recepção) — valor atômico, sem JSON aninhado.
/// </summary>
public class AcaoEventoEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Servico { get; set; } = string.Empty;
    public string Icone { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;

    [ForeignKey(nameof(Evento))]
    public string EventoReservaId { get; set; } = string.Empty;
    public virtual EventoReservaEntity? Evento { get; set; }
}
