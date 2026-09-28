namespace SistemaHotelaria.Services.Iterator;

/// <summary>
/// Critérios aplicados pelo Iterator ao percorrer a coleção de reservas.
/// </summary>
public class FiltroReservas
{
    public string? Status { get; set; }
    public string? TipoQuarto { get; set; }
    public string? NomeHospede { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public bool EstaVazio =>
        string.IsNullOrWhiteSpace(Status)
        && string.IsNullOrWhiteSpace(TipoQuarto)
        && string.IsNullOrWhiteSpace(NomeHospede)
        && DataInicio == null
        && DataFim == null;
}
