using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Iterator;

/// <summary>
/// Iterator: percorre reservas sem expor a estrutura interna da coleção.
/// </summary>
public interface IReservaIterator
{
    bool TemProximo();
    Reserva Proximo();
    void Resetar();
}
