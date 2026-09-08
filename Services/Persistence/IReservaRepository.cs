using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Persistence;

/// <summary>
/// Adapter: contrato de persistência independente da implementação (memória, banco, API).
/// </summary>
public interface IReservaRepository
{
    IReadOnlyList<Reserva> ObterTodas();
    Reserva? ObterPorId(string id);
    void Adicionar(Reserva reserva);
    void Atualizar(Reserva reserva);
    int ContarConflitos(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida);
}
