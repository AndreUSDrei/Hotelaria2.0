using SistemaHotelaria.Builder;
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
    
    // ============================================================
    // NOVO MÉTODO - Integração com Padrão Decorator
    // ============================================================
    // Este método permite criar uma reserva com pacote base + 
    // serviços extras adicionados via Decorator Pattern.
    //
    // IMPORTANTE - PARÂMETRO decorators:
    // É uma lista de strings onde cada string representa um decorator
    // a ser aplicado. Ex: ["Spa", "Transfer", "LateCheckout"]
    //
    // Isso demonstra como o padrão Decorator se integra com outros
    // padrões (Builder, Facade) de forma harmoniosa.
    // ============================================================
    Reserva? CriarReservaComPacoteEDecorators(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida, List<string> decorators);
    
    bool RealizarCheckIn(string id);
    bool RealizarCheckOut(string id);
    List<IQuarto> ObterPrototiposQuartos();
    string[] ObterTiposPacote();
    Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida);
    int ContarReservasAtivas();
}
