using System.ComponentModel.DataAnnotations;

namespace SistemaHotelaria.Models;

public class ReservaEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string HospedeNome { get; set; } = string.Empty;
    public string TipoQuarto { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public decimal ValorTotal { get; set; }
    public bool CheckInRealizado { get; set; }
    public bool CheckOutRealizado { get; set; }
    public string PacoteNome { get; set; } = string.Empty;
    public string PacoteDescricao { get; set; } = string.Empty;
    public decimal PacoteDescontoPercentual { get; set; }
    public string MetodoPagamento { get; set; } = "Pix";
}
