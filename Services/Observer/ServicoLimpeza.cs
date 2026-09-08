using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Observer;

public class ServicoLimpeza : IObserver
{
    public void Atualizar(Reserva reserva)
    {
        var descricao = reserva.Status switch
        {
            "Check-in" => $"Quarto {reserva.TipoQuarto} ocupado — arrumação diária programada.",
            "Check-out" => $"Quarto {reserva.TipoQuarto} liberado — faxina completa agendada.",
            _ => $"Quarto {reserva.TipoQuarto} reservado — preparação antes da chegada."
        };

        Console.WriteLine($"🧹 [Limpeza] {descricao}");
        reserva.RegistrarAcao("Limpeza", "🧹", descricao);
    }
}
