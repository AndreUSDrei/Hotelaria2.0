using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Observer;

public class Recepcao : IObserver
{
    public void Atualizar(Reserva reserva)
    {
        var descricao = reserva.Status switch
        {
            "Check-in" => $"{reserva.HospedeNome} fez check-in — chaves entregues.",
            "Check-out" => $"{reserva.HospedeNome} fez check-out — conta encerrada no sistema.",
            _ => $"Reserva #{reserva.Id} de {reserva.HospedeNome} disponível no painel da recepção."
        };

        Console.WriteLine($"🛎️ [Recepção] {descricao}");
        reserva.RegistrarAcao("Recepção", "🛎️", descricao);
    }
}
