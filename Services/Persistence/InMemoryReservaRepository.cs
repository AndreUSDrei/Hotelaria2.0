using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Persistence;

/// <summary>
/// Adapter: adapta o armazenamento em memória (List) à interface IReservaRepository.
/// </summary>
public class InMemoryReservaRepository : IReservaRepository
{
    private readonly List<Reserva> _reservas = new();

    public IReadOnlyList<Reserva> ObterTodas() => _reservas;

    public Reserva? ObterPorId(string id) => _reservas.FirstOrDefault(r => r.Id == id);

    public void Adicionar(Reserva reserva) => _reservas.Add(reserva);

    public int ContarConflitos(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida) =>
        _reservas.Count(r =>
            r.TipoQuarto == tipoQuarto &&
            !r.CheckOutRealizado &&
            r.DataEntrada < dataSaida &&
            r.DataSaida > dataEntrada);
}
