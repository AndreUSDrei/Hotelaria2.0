namespace SistemaHotelaria.Services.Strategy;

/// <summary>
/// Strategy Pattern: Concrete strategy - Credit Card payment.
/// </summary>
public class PagamentoCartaoCredito : IEstrategiaPagamento
{
    public string Nome => "Cartão de Crédito";

    public ResultadoPagamento Pagar(decimal valor)
    {
        var transacaoId = $"AUT-{Random.Shared.Next(100000, 999999)}";
        var mensagem = $"Pagamento no cartão autorizado em 1x de R$ {valor:N2}.";

        Console.WriteLine($"💳 [Cartão] {mensagem} ({transacaoId})");

        return new ResultadoPagamento
        {
            Sucesso = true,
            Metodo = Nome,
            TransacaoId = transacaoId,
            Mensagem = mensagem
        };
    }
}
