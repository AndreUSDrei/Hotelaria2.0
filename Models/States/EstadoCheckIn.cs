namespace SistemaHotelaria.Models.States;

public class EstadoCheckIn : IEstadoReserva
{
    public string Nome => "Check-in";

    public void CheckIn(Reserva reserva)
    {
        throw new InvalidOperationException("O check-in já foi realizado.");
    }

    public void CheckOut(Reserva reserva)
    {
        reserva.AtualizarEstado(new EstadoCheckOut());
    }

    public void Cancelar(Reserva reserva)
    {
        throw new InvalidOperationException("Não é possível cancelar uma reserva após o check-in. Faça o check-out.");
    }
}
