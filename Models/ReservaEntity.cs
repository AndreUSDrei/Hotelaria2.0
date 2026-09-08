using System.ComponentModel.DataAnnotations;

namespace SistemaHotelaria.Models;

public class ReservaEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public decimal ValorTotal { get; set; }
    public string MetodoPagamento { get; set; } = "Pix";
    public string PagamentoTransacaoId { get; set; } = string.Empty;
    public string PagamentoComprovante { get; set; } = string.Empty;
    public string StatusReserva { get; set; } = "Pendente";
    public string EventosReserva { get; set; } = string.Empty;

    // Chaves Estrangeiras 3NF
    public string HospedeId { get; set; } = string.Empty;
    public virtual HospedeEntity? Hospede { get; set; }

    public string TipoQuartoId { get; set; } = string.Empty;
    public virtual TipoQuartoEntity? TipoQuarto { get; set; }

    public string? PacoteId { get; set; }
    public virtual PacoteEntity? Pacote { get; set; }
}
