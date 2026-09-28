namespace SistemaHotelaria.Models.States;

public interface IEstadoReserva
{
    string Nome { get; }
    void CheckIn(Reserva reserva);
    void CheckOut(Reserva reserva);
    void Cancelar(Reserva reserva);
}
