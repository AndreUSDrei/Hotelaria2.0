namespace SistemaHotelaria.Services.Strategy;

public class PagamentoBoleto : IEstrategiaPagamento
{
    public string Nome => "Boleto";
    public string CodigoBarras { get; init; } = "34191.79001 01043.510047 91020.150008 8 71070000005000";

    public ResultadoPagamento Pagar(decimal valor)
    {
        var transacaoId = $"BOL-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
        var vencimento = DateTime.Today.AddDays(3).ToString("dd/MM/yyyy");
        var codigo = GerarCodigoBarras(valor);
        var mensagem = $"Boleto de R$ {valor:N2} gerado (simulação compensada). Vencimento {vencimento}.";

        Console.WriteLine($"🏷️ [Boleto] {mensagem} ({transacaoId})");

        return new ResultadoPagamento
        {
            Sucesso = true,
            Metodo = Nome,
            TransacaoId = transacaoId,
            Mensagem = mensagem,
            Detalhes = new Dictionary<string, string>
            {
                ["Código de barras"] = codigo,
                ["Vencimento"] = vencimento,
                ["Cedente"] = "Hotelaria Luxury S.A.",
                ["Status"] = "Compensado (simulação)",
                ["Horário"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            }
        };
    }

    private string GerarCodigoBarras(decimal valor)
    {
        var valorCentavos = ((int)(valor * 100)).ToString("D10");
        return $"34191.79001 01043.510047 91020.150008 8 {valorCentavos}";
    }
}
