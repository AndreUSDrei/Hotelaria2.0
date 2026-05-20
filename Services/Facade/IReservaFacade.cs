using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Services.Facade;

/// <summary>
/// Facade: interface unificada para o subsistema de reservas (Builder, Prototype, disponibilidade).
/// </summary>
public interface IReservaFacade
{
    List<Reserva> ObterTodasReservas();
    Reserva? ObterReservaPorId(string id);
    Reserva? CriarReservaComPacote(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida);
    bool RealizarCheckIn(string id);
    bool RealizarCheckOut(string id);
    List<IQuarto> ObterPrototiposQuartos();
    string[] ObterTiposPacote();
    Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida);
    int ContarReservasAtivas();
}
