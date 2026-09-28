using SistemaHotelaria.Builder;
using SistemaHotelaria.Decorator;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;
using SistemaHotelaria.Services.Iterator;

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

    public List<Reserva> ObterReservasFiltradas(FiltroReservas filtro) =>
        _gerenciador.ObterReservasFiltradas(filtro);

    public Reserva? ObterReservaPorId(string id) => _gerenciador.ObterReservaPorId(id);

    public bool RealizarCheckIn(string id) => _gerenciador.RealizarCheckIn(id);

    public bool RealizarCheckOut(string id) => _gerenciador.RealizarCheckOut(id);

    public bool CancelarReserva(string id) => _gerenciador.CancelarReserva(id);

    public List<IQuarto> ObterPrototiposQuartos() => _hotelService.ObterPrototiposQuartos();

    public string[] ObterTiposPacote() => ["Romantico", "Negocios", "Basico", "FimDeSemana"];

    public Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida) =>
        _gerenciador.ObterDisponibilidadeCompleta(entrada, saida);

    public int ContarReservasAtivas() => _gerenciador.ObterTodasReservas().Count;

    public ResultadoCriacaoReserva CriarReservaComPacote(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida, string metodoPagamento = "pix",
        string? numeroCartao = null, string? cvv = null)
    {
        var tipoQuartoFinal = tipoQuarto;
        if (!string.IsNullOrEmpty(tipoPacote) && string.IsNullOrEmpty(tipoQuarto))
            tipoQuartoFinal = "Standard";

        var quarto = _hotelService.ObterPrototipoPorTipo(tipoQuartoFinal);
        if (quarto == null)
            return ResultadoCriacaoReserva.Falha("Tipo de quarto não encontrado.");

        var builder = _hotelService.CriarBuilder(tipoPacote);
        var director = new HotelDirector(builder);
        ConstruirPacote(director, tipoPacote, quarto);

        return _gerenciador.CriarReservaWeb(hospedeNome, tipoQuartoFinal, dataEntrada, dataSaida, director.ObterPacote(),
            metodoPagamento, numeroCartao, cvv);
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

    public ResultadoCriacaoReserva CriarReservaComPacoteEDecorators(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida, List<string> decorators, string metodoPagamento = "pix",
        string? numeroCartao = null, string? cvv = null)
    {
        var tipoQuartoFinal = tipoQuarto;
        if (!string.IsNullOrEmpty(tipoPacote) && string.IsNullOrEmpty(tipoQuarto))
            tipoQuartoFinal = "Standard";

        var quarto = _hotelService.ObterPrototipoPorTipo(tipoQuartoFinal);
        if (quarto == null)
            return ResultadoCriacaoReserva.Falha("Tipo de quarto não encontrado.");

        var builder = _hotelService.CriarBuilder(tipoPacote);
        var director = new HotelDirector(builder);
        ConstruirPacote(director, tipoPacote, quarto);

        var pacote = director.ObterPacote();
        IPacoteDecorator? decoratorAtual = null;

        foreach (var nomeDecorator in decorators)
        {
            switch (nomeDecorator.ToLower())
            {
                case "spa":
                    decoratorAtual = new SpaDecorator(pacote);
                    break;
                case "transfer":
                    decoratorAtual = new TransferDecorator(pacote);
                    break;
                case "latecheckout":
                    decoratorAtual = new LateCheckoutDecorator(pacote);
                    break;
                case "petfriendly":
                    decoratorAtual = new PetFriendlyDecorator(pacote);
                    break;
                default:
                    continue;
            }

            decoratorAtual?.AdicionarServicoExtra();
        }

        int dias = (dataSaida - dataEntrada).Days;
        decimal valorTotal = decoratorAtual != null
            ? decoratorAtual.CalcularValorTotalComDecorators(dias)
            : pacote.CalcularValorTotal(dias);

        var resultado = _gerenciador.CriarReservaWeb(hospedeNome, tipoQuartoFinal, dataEntrada, dataSaida, pacote,
            metodoPagamento, numeroCartao, cvv);

        if (resultado.Sucesso && resultado.Reserva != null)
        {
            resultado.Reserva.ValorTotal = valorTotal;
            _gerenciador.AtualizarReserva(resultado.Reserva);
        }

        return resultado;
    }
}
