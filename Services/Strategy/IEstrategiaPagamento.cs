namespace SistemaHotelaria.Services.Strategy;

public interface IEstrategiaPagamento
{
    string Nome { get; }
    ResultadoPagamento Pagar(decimal valor);
}
