namespace SistemaHotelaria.Services.Strategy;

public interface IEstrategiaPagamento
{
    string Nome { get; }
    bool Pagar(decimal valor);
}
