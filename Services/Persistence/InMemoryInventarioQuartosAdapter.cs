namespace SistemaHotelaria.Services.Persistence;

/// <summary>
/// Adapter: adapta o inventário fixo em memória ao contrato IInventarioQuartos.
/// </summary>
public class InMemoryInventarioQuartosAdapter : IInventarioQuartos
{
    private static readonly Dictionary<string, int> Inventario = new()
    {
        { "Standard", 20 },
        { "Luxo", 10 },
        { "Suíte Presidencial", 3 }
    };

    public IReadOnlyDictionary<string, int> ObterCapacidadePorTipo() => Inventario;
}
