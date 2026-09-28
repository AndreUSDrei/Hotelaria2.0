using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHotelaria.Models;

/// <summary>
/// Pagamento 1:1 com Reserva — atributos de pagamento não transitam via outros campos da reserva (3FN).
/// </summary>
public class PagamentoEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Metodo { get; set; } = "Pix";
    public string TransacaoId { get; set; } = string.Empty;
    public string Comprovante { get; set; } = string.Empty;

    [ForeignKey(nameof(Reserva))]
    public string ReservaId { get; set; } = string.Empty;
    public virtual ReservaEntity? Reserva { get; set; }
}
