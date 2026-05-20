namespace SistemaHotelaria.Services.Persistence;

/// <summary>
/// Adapter: contrato do inventário de quartos por tipo (independente da fonte de dados).
/// </summary>
public interface IInventarioQuartos
{
    IReadOnlyDictionary<string, int> ObterCapacidadePorTipo();
}
