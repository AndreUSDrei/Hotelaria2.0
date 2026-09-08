using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Observer;

public class ServicoEmail : IObserver
{
    public void Atualizar(Reserva reserva)
    {
        var descricao = reserva.Status switch
        {
            "Check-in" => $"Confirmação de check-in enviada para {reserva.HospedeNome}.",
            "Check-out" => $"Comprovante de estadia e agradecimento enviados para {reserva.HospedeNome}.",
            _ => $"Confirmação da reserva #{reserva.Id} enviada para {reserva.HospedeNome}."
        };

        Console.WriteLine($"📧 [E-mail] {descricao}");
        reserva.RegistrarAcao("E-mail", "📧", descricao);
    }
}
