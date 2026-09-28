namespace SistemaHotelaria.Models.States;

public class EstadoCheckOut : IEstadoReserva
{
    public string Nome => "Check-out";

    public void CheckIn(Reserva reserva)
    {
        throw new InvalidOperationException("A reserva já foi finalizada (Check-out realizado).");
    }

    public void CheckOut(Reserva reserva)
    {
        throw new InvalidOperationException("O check-out já foi realizado.");
    }

    public void Cancelar(Reserva reserva)
    {
        throw new InvalidOperationException("Não é possível cancelar uma reserva que já teve o check-out realizado.");
    }
}
