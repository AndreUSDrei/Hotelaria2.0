namespace SistemaHotelaria.Services.Strategy;

/// <summary>
/// Strategy Pattern: Concrete strategy - Pix payment.
/// </summary>
public class PagamentoPix : IEstrategiaPagamento
{
    public string Nome => "Pix";

    public ResultadoPagamento Pagar(decimal valor)
    {
        var transacaoId = $"PIX-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        var mensagem = $"Pagamento Pix de R$ {valor:N2} aprovado instantaneamente.";

        Console.WriteLine($"💸 [Pix] {mensagem} ({transacaoId})");

        return new ResultadoPagamento
        {
            Sucesso = true,
            Metodo = Nome,
            TransacaoId = transacaoId,
            Mensagem = mensagem
        };
    }
}
