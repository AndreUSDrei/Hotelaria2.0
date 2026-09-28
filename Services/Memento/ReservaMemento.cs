namespace SistemaHotelaria.Services.Memento;

public class ReservaMemento
{
    public string Id { get; }
    public string HospedeNome { get; }
    public string TipoQuarto { get; }
    public DateTime DataEntrada { get; }
    public DateTime DataSaida { get; }
    public decimal ValorTotal { get; }
    public string Status { get; }
    public string MetodoPagamento { get; }
    public string PagamentoTransacaoId { get; }
    public string PagamentoComprovante { get; }
    public DateTime Timestamp { get; }

    public ReservaMemento(
        string id, string hospedeNome, string tipoQuarto,
        DateTime dataEntrada, DateTime dataSaida, decimal valorTotal,
        string status, string metodoPagamento, string pagamentoTransacaoId,
        string pagamentoComprovante)
    {
        Id = id;
        HospedeNome = hospedeNome;
        TipoQuarto = tipoQuarto;
        DataEntrada = dataEntrada;
        DataSaida = dataSaida;
        ValorTotal = valorTotal;
        Status = status;
        MetodoPagamento = metodoPagamento;
        PagamentoTransacaoId = pagamentoTransacaoId;
        PagamentoComprovante = pagamentoComprovante;
        Timestamp = DateTime.Now;
    }
}