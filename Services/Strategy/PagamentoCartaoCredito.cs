namespace SistemaHotelaria.Services.Strategy;

public class PagamentoCartaoCredito : IEstrategiaPagamento
{
    public string Nome => "Cartão de Crédito";
    public string NumeroCartao { get; init; } = "4111 1111 1111 1111";
    public string Cvv { get; init; } = "123";

    public ResultadoPagamento Pagar(decimal valor)
    {
        var transacaoId = $"AUT-{Random.Shared.Next(100000, 999999)}";
        var final = NumeroMascarado;
        var mensagem = $"Pagamento no cartão **** {final} autorizado em 1x de R$ {valor:N2}.";

        Console.WriteLine($"💳 [Cartão] {mensagem} ({transacaoId})");

        return new ResultadoPagamento
        {
            Sucesso = true,
            Metodo = Nome,
            TransacaoId = transacaoId,
            Mensagem = mensagem,
            Detalhes = new Dictionary<string, string>
            {
                ["Cartão"] = $"**** **** **** {final}",
                ["CVV"] = "***",
                ["Parcelas"] = $"1x de R$ {valor:N2}",
                ["Status"] = "Autorizado pela operadora",
                ["Horário"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            }
        };
    }

    private string NumeroMascarado
    {
        get
        {
            var digits = new string(NumeroCartao.Where(char.IsDigit).ToArray());
            return digits.Length >= 4 ? digits[^4..] : "1111";
        }
    }
}
