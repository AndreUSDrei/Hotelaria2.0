namespace SistemaHotelaria.Models.States;

public class EstadoConfirmada : IEstadoReserva
{
    public string Nome => "Confirmada";

    public void CheckIn(Reserva reserva)
    {
        reserva.AtualizarEstado(new EstadoCheckIn());
    }

    public void CheckOut(Reserva reserva)
    {
        throw new InvalidOperationException("Não é possível fazer check-out sem antes fazer o check-in.");
    }

    public void Cancelar(Reserva reserva)
    {
        reserva.AtualizarEstado(new EstadoCancelada());
    }
}
