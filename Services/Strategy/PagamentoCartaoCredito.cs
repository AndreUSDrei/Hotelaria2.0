namespace SistemaHotelaria.Services.Strategy;

public class PagamentoCartaoCredito : IEstrategiaPagamento
{
    public string Nome => "Cartão de Crédito";
    public string NumeroCartao { get; init; } = "0000 0000 0000 0000";
    public string Cvv { get; init; } = "000";

    public bool Pagar(decimal valor)
    {
        Console.WriteLine($"💳 [Cartão] Processando pagamento de R$ {valor:N2} no cartão {NumeroCartao}...");
        return true;
    }
}
