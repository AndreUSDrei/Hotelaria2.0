namespace SistemaHotelaria.Models;

/// <summary>
/// Define os tipos de quartos e seus preços diários.
/// </summary>
public static class TipoQuarto
{
    public const string Standard = "Standard";
    public const string Luxo = "Luxo";
    public const string Suite = "Suíte";

    private static readonly Dictionary<string, decimal> PrecosDiarios = new()
    {
        { Standard, 250.00m },
        { Luxo, 550.00m },
        { Suite, 1050.00m }
    };

    public static decimal ObterPrecoDiario(string tipoQuarto)
    {
        return PrecosDiarios.TryGetValue(tipoQuarto, out var preco) ? preco : 250.00m;
    }

    public static string[] ObterTodosTipos() => new[] { Standard, Luxo, Suite };
}
