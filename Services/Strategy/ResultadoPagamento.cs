namespace SistemaHotelaria.Services.Strategy;

public class ResultadoPagamento
{
    public bool Sucesso { get; init; }
    public string Metodo { get; init; } = string.Empty;
    public string TransacaoId { get; init; } = string.Empty;
    public string Mensagem { get; init; } = string.Empty;
    public Dictionary<string, string> Detalhes { get; init; } = new();

    public string ComprovanteTexto
    {
        get
        {
            var linhas = new List<string>
            {
                Mensagem,
                $"Método: {Metodo}",
                $"Transação: {TransacaoId}"
            };
            linhas.AddRange(Detalhes.Select(d => $"{d.Key}: {d.Value}"));
            return string.Join(" | ", linhas);
        }
    }
}
