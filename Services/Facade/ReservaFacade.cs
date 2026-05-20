using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Services.Facade;

/// <summary>
/// Facade: simplifica a criação de reservas ocultando Builder, Director, Prototype e gerenciamento.
/// </summary>
public class ReservaFacade : IReservaFacade
{
    private readonly IGerenciadorReservas _gerenciador;
    private readonly HotelService _hotelService;

    public ReservaFacade(IGerenciadorReservas gerenciador, HotelService hotelService)
    {
        _gerenciador = gerenciador;
        _hotelService = hotelService;
    }

    public List<Reserva> ObterTodasReservas() => _gerenciador.ObterTodasReservas();

    public Reserva? ObterReservaPorId(string id) => _gerenciador.ObterReservaPorId(id);

    public bool RealizarCheckIn(string id) => _gerenciador.RealizarCheckIn(id);

    public bool RealizarCheckOut(string id) => _gerenciador.RealizarCheckOut(id);

    public List<IQuarto> ObterPrototiposQuartos() => _hotelService.ObterPrototiposQuartos();

    public string[] ObterTiposPacote() => ["Romantico", "Negocios", "Basico", "FimDeSemana"];

    public Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida) =>
        _gerenciador.ObterDisponibilidadeCompleta(entrada, saida);

    public int ContarReservasAtivas() => _gerenciador.ObterTodasReservas().Count;

    public Reserva? CriarReservaComPacote(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida)
    {
        var tipoQuartoFinal = tipoQuarto;
        if (!string.IsNullOrEmpty(tipoPacote) && string.IsNullOrEmpty(tipoQuarto))
            tipoQuartoFinal = "Standard";

        var quarto = _hotelService.ObterPrototipoPorTipo(tipoQuartoFinal);
        if (quarto == null)
            return null;

        var builder = _hotelService.CriarBuilder(tipoPacote);
        var director = new HotelDirector(builder);
        ConstruirPacote(director, tipoPacote, quarto);

        return _gerenciador.CriarReservaWeb(hospedeNome, tipoQuartoFinal, dataEntrada, dataSaida, director.ObterPacote());
    }

    private static void ConstruirPacote(HotelDirector director, string tipoPacote, IQuarto quarto)
    {
        switch (tipoPacote)
        {
            case "Romantico":
                director.ConstruirPacoteRomanticoCompleto(quarto);
                break;
            case "Negocios":
                director.ConstruirPacoteNegociosCompleto(quarto);
                break;
            case "Basico":
                director.ConstruirPacoteBasico(quarto);
                break;
            case "FimDeSemana":
                director.ConstruirPacoteFimDeSemana(quarto);
                break;
            default:
                director.ConstruirPacoteBasico(quarto);
                break;
        }
    }
}
