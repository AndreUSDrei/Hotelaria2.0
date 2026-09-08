namespace SistemaHotelaria.Services.Strategy;

/// <summary>
/// Strategy Pattern: Concrete strategy - Boleto payment.
/// </summary>
public class PagamentoBoleto : IEstrategiaPagamento
{
    public string Nome => "Boleto";

    public ResultadoPagamento Pagar(decimal valor)
    {
        var transacaoId = $"BOL-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
        var vencimento = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");
        var mensagem = $"Boleto de R$ {valor:N2} gerado. Vencimento {vencimento}.";

        Console.WriteLine($"🏷️ [Boleto] {mensagem} ({transacaoId})");

        return new ResultadoPagamento
        {
            Sucesso = true,
            Metodo = Nome,
            TransacaoId = transacaoId,
            Mensagem = mensagem
        };
    }
}
