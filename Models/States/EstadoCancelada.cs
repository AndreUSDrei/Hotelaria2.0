namespace SistemaHotelaria.Models.States;

public class EstadoCancelada : IEstadoReserva
{
    public string Nome => "Cancelada";

    public void CheckIn(Reserva reserva)
    {
        throw new InvalidOperationException("Não é possível fazer check-in de uma reserva cancelada.");
    }

    public void CheckOut(Reserva reserva)
    {
        throw new InvalidOperationException("Não é possível fazer check-out de uma reserva cancelada.");
    }

    public void Cancelar(Reserva reserva)
    {
        throw new InvalidOperationException("A reserva já está cancelada.");
    }
}
