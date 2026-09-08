namespace SistemaHotelaria.Services.Strategy;

/// <summary>
/// Strategy Pattern: Result of payment processing.
/// </summary>
public class ResultadoPagamento
{
    public bool Sucesso { get; init; }
    public string Metodo { get; init; } = string.Empty;
    public string TransacaoId { get; init; } = string.Empty;
    public string Mensagem { get; init; } = string.Empty;
}
