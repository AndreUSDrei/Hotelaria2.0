using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Observer;

public class RecepcaoObserver : IObserver
{
    public void Atualizar(Reserva reserva)
    {
        Console.WriteLine($"🛎️ [Recepção] Informando a recepção sobre a reserva {reserva.Id} do hóspede {reserva.HospedeNome}.");
    }
}
