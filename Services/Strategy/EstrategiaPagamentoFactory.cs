namespace SistemaHotelaria.Services.Strategy;

/// <summary>
/// Strategy Pattern: Factory for creating payment strategies.
/// </summary>
public static class EstrategiaPagamentoFactory
{
    public static IEstrategiaPagamento Criar(string metodo)
    {
        return (metodo ?? "pix").Trim().ToLower() switch
        {
            "cartao" or "cartão" => new PagamentoCartaoCredito(),
            "boleto" => new PagamentoBoleto(),
            _ => new PagamentoPix()
        };
    }
}
