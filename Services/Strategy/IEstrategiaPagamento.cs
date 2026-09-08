namespace SistemaHotelaria.Services.Strategy;

/// <summary>
/// Strategy Pattern: Strategy interface for payment methods.
/// </summary>
public interface IEstrategiaPagamento
{
    string Nome { get; }
    ResultadoPagamento Pagar(decimal valor);
}
