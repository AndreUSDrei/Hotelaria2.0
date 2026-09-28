using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Iterator;

/// <summary>
/// Aggregate do padrão Iterator: coleção que cria iterators filtráveis.
/// </summary>
public class ColecaoReservas
{
    private readonly List<Reserva> _reservas;

    public ColecaoReservas(IEnumerable<Reserva> reservas)
    {
        _reservas = reservas.ToList();
    }

    public int Total => _reservas.Count;

    public IReservaIterator CriarIterator(FiltroReservas? filtro = null) =>
        new ReservaIterator(_reservas, filtro);

    /// <summary>Materializa o percurso do iterator em lista (útil para Views MVC).</summary>
    public List<Reserva> ParaLista(FiltroReservas? filtro = null)
    {
        var iterator = CriarIterator(filtro);
        var resultado = new List<Reserva>();
        while (iterator.TemProximo())
            resultado.Add(iterator.Proximo());
        return resultado;
    }
}
