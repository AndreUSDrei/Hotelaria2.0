using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Iterator;

/// <summary>
/// Iterator concreto: avança apenas sobre reservas que passam no filtro.
/// </summary>
public class ReservaIterator : IReservaIterator
{
    private readonly IReadOnlyList<Reserva> _reservas;
    private readonly FiltroReservas _filtro;
    private int _indice = -1;

    public ReservaIterator(IReadOnlyList<Reserva> reservas, FiltroReservas? filtro = null)
    {
        _reservas = reservas;
        _filtro = filtro ?? new FiltroReservas();
    }

    public bool TemProximo()
    {
        var proximo = _indice + 1;
        while (proximo < _reservas.Count)
        {
            if (AtendeFiltro(_reservas[proximo]))
                return true;
            proximo++;
        }
        return false;
    }

    public Reserva Proximo()
    {
        _indice++;
        while (_indice < _reservas.Count)
        {
            if (AtendeFiltro(_reservas[_indice]))
                return _reservas[_indice];
            _indice++;
        }

        throw new InvalidOperationException("Não há mais reservas no iterator.");
    }

    public void Resetar() => _indice = -1;

    private bool AtendeFiltro(Reserva reserva)
    {
        if (!string.IsNullOrWhiteSpace(_filtro.Status)
            && !reserva.Status.Equals(_filtro.Status, StringComparison.OrdinalIgnoreCase))
            return false;

        if (!string.IsNullOrWhiteSpace(_filtro.TipoQuarto)
            && !reserva.TipoQuarto.Equals(_filtro.TipoQuarto, StringComparison.OrdinalIgnoreCase))
            return false;

        if (!string.IsNullOrWhiteSpace(_filtro.NomeHospede)
            && !reserva.HospedeNome.Contains(_filtro.NomeHospede, StringComparison.OrdinalIgnoreCase))
            return false;

        if (_filtro.DataInicio.HasValue && reserva.DataSaida.Date < _filtro.DataInicio.Value.Date)
            return false;

        if (_filtro.DataFim.HasValue && reserva.DataEntrada.Date > _filtro.DataFim.Value.Date)
            return false;

        return true;
    }
}
