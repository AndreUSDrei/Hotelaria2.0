using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Observer;

public class LimpezaObserver : IObserver
{
    public void Atualizar(Reserva reserva)
    {
        Console.WriteLine($"🧹 [Limpeza] Agendando serviço de limpeza para o quarto do hóspede {reserva.HospedeNome} (reserva {reserva.Id}).");
    }
}
