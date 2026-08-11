using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Observer;

public class EmailObserver : IObserver
{
    public void Atualizar(Reserva reserva)
    {
        Console.WriteLine($"📧 [Email] Enviando notificação para hóspede {reserva.HospedeNome} sobre a reserva {reserva.Id}.");
    }
}
