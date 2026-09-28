using SistemaHotelaria.Models;
using SistemaHotelaria.Services;
using SistemaHotelaria.Services.Memento;

namespace SistemaHotelaria.Services.Commands;

public class CheckOutCommand : ICommand
{
    private readonly IGerenciadorReservas _gerenciadorReservas;
    private readonly string _idReserva;
    private ReservaMemento? _estadoAnterior;
    private bool _executado;

    public CheckOutCommand(IGerenciadorReservas gerenciadorReservas, string idReserva)
    {
        _gerenciadorReservas = gerenciadorReservas;
        _idReserva = idReserva;
        _executado = false;
    }

    public void Execute()
    {
        if (!_executado)
        {
            var reserva = _gerenciadorReservas.ObterReservaPorId(_idReserva);
            if (reserva == null)
                return;

            _estadoAnterior = reserva.CriarMemento();
            _gerenciadorReservas.RealizarCheckOut(_idReserva);
            _executado = true;
        }
    }

    public void Undo()
    {
        if (_executado && _estadoAnterior != null)
        {
            var reserva = _gerenciadorReservas.ObterReservaPorId(_idReserva);
            if (reserva != null)
            {
                reserva.RestaurarMemento(_estadoAnterior);
                _gerenciadorReservas.AtualizarReserva(reserva);
                _executado = false;
            }
        }
    }
}