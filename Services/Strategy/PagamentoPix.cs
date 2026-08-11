namespace SistemaHotelaria.Services.Strategy;

public class PagamentoPix : IEstrategiaPagamento
{
    public string Nome => "Pix";

    public bool Pagar(decimal valor)
    {
        Console.WriteLine($"💸 [Pix] Processando pagamento de R$ {valor:N2} via Pix...");
        return true;
    }
}
