namespace SistemaHotelaria.Services.Strategy;

public class PagamentoBoleto : IEstrategiaPagamento
{
    public string Nome => "Boleto";
    public string CodigoBarras { get; init; } = "34191.79001 01043.510047 91020.150008 8 71070000005000";

    public bool Pagar(decimal valor)
    {
        Console.WriteLine($"🏷️ [Boleto] Gerando boleto de R$ {valor:N2} ({CodigoBarras})...");
        return true;
    }
}
