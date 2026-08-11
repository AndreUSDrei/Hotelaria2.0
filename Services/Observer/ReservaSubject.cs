using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Observer;

public class ReservaSubject : ISubject
{
    private readonly List<IObserver> _observadores = new();
    public Reserva Reserva { get; private set; }

    public ReservaSubject(Reserva reserva)
    {
        Reserva = reserva;
    }

    public void Anexar(IObserver observador)
    {
        if (!_observadores.Contains(observador))
            _observadores.Add(observador);
    }

    public void Desanexar(IObserver observador)
    {
        _observadores.Remove(observador);
    }

    public void Notificar()
    {
        foreach (var observador in _observadores)
        {
            observador.Atualizar(Reserva);
        }
    }
}
