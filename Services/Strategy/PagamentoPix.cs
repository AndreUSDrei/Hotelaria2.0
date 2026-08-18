namespace SistemaHotelaria.Services.Strategy;

public class PagamentoPix : IEstrategiaPagamento
{
    public string Nome => "Pix";
    public string ChavePix { get; init; } = "hotelaria.luxury@pix.com.br";

    public ResultadoPagamento Pagar(decimal valor)
    {
        var transacaoId = $"PIX-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        var mensagem = $"Pagamento Pix de R$ {valor:N2} aprovado instantaneamente.";

        Console.WriteLine($"💸 [Pix] {mensagem} Chave: {ChavePix} ({transacaoId})");

        return new ResultadoPagamento
        {
            Sucesso = true,
            Metodo = Nome,
            TransacaoId = transacaoId,
            Mensagem = mensagem,
            Detalhes = new Dictionary<string, string>
            {
                ["Chave Pix"] = ChavePix,
                ["Tipo"] = "Transferência instantânea",
                ["Status"] = "Aprovado",
                ["Horário"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            }
        };
    }
}
