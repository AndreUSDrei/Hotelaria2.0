using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;
using SistemaHotelaria.Services.Iterator;

namespace SistemaHotelaria.Services.Facade;

/// <summary>
/// Facade: interface unificada para o subsistema de reservas (Builder, Prototype, disponibilidade).
/// </summary>
public interface IReservaFacade
{
    List<Reserva> ObterTodasReservas();
    List<Reserva> ObterReservasFiltradas(FiltroReservas filtro);
    Reserva? ObterReservaPorId(string id);
    ResultadoCriacaoReserva CriarReservaComPacote(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida, string metodoPagamento = "pix",
        string? numeroCartao = null, string? cvv = null);

    ResultadoCriacaoReserva CriarReservaComPacoteEDecorators(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida, List<string> decorators, string metodoPagamento = "pix",
        string? numeroCartao = null, string? cvv = null);

    bool RealizarCheckIn(string id);
    bool RealizarCheckOut(string id);
    bool CancelarReserva(string id);
    List<IQuarto> ObterPrototiposQuartos();
    string[] ObterTiposPacote();
    Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida);
    int ContarReservasAtivas();
}
