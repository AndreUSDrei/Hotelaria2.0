using SistemaHotelaria.Services;

namespace SistemaHotelaria.Services.Commands;

public class CheckInCommand : ICommand
{
    private readonly IGerenciadorReservas _gerenciadorReservas;
    private readonly string _idReserva;
    private bool _executado;

    public CheckInCommand(IGerenciadorReservas gerenciadorReservas, string idReserva)
    {
        _gerenciadorReservas = gerenciadorReservas;
        _idReserva = idReserva;
        _executado = false;
    }

    public void Execute()
    {
        if (_executado)
            return;

        _executado = _gerenciadorReservas.RealizarCheckIn(_idReserva);
    }

    public void Undo()
    {
        if (!_executado)
            return;

        _executado = !_gerenciadorReservas.RestaurarEstadoAnterior(_idReserva);
    }
}
