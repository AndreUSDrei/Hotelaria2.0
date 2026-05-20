using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Services.Proxies;

/// <summary>
/// Proxy: validação de datas e registro de operações antes de delegar ao GerenciadorReservas real.
/// </summary>
public class GerenciadorReservasProxy : IGerenciadorReservas
{
    private readonly IGerenciadorReservas _real;
    private readonly ILogger<GerenciadorReservasProxy> _logger;

    public GerenciadorReservasProxy(GerenciadorReservas real, ILogger<GerenciadorReservasProxy> logger)
    {
        _real = real;
        _logger = logger;
    }

    public bool QuartoDisponivel(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        if (!DatasValidas(dataEntrada, dataSaida))
            return false;

        _logger.LogDebug("Consultando disponibilidade: {Tipo} {Entrada:d} - {Saida:d}", tipoQuarto, dataEntrada, dataSaida);
        return _real.QuartoDisponivel(tipoQuarto, dataEntrada, dataSaida);
    }

    public int ObterQuantidadeQuartosDisponiveis(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        if (!DatasValidas(dataEntrada, dataSaida))
            return 0;

        return _real.ObterQuantidadeQuartosDisponiveis(tipoQuarto, dataEntrada, dataSaida);
    }

    public Reserva? CriarReserva(string nomeHospede, IQuarto quartoBase, IPacoteHospedagemBuilder builder,
        HotelDirector director, DateTime entrada, DateTime saida)
    {
        if (!DatasValidas(entrada, saida))
            return null;

        _logger.LogInformation("Criando reserva (console) para {Hospede}", nomeHospede);
        return _real.CriarReserva(nomeHospede, quartoBase, builder, director, entrada, saida);
    }

    public bool RealizarCheckIn(string idReserva)
    {
        _logger.LogInformation("Check-in solicitado: {Id}", idReserva);
        return _real.RealizarCheckIn(idReserva);
    }

    public bool RealizarCheckOut(string idReserva)
    {
        _logger.LogInformation("Check-out solicitado: {Id}", idReserva);
        return _real.RealizarCheckOut(idReserva);
    }

    public void ListarReservas() => _real.ListarReservas();

    public void ExibirDisponibilidade(DateTime dataEntrada, DateTime dataSaida) =>
        _real.ExibirDisponibilidade(dataEntrada, dataSaida);

    public List<Reserva> ObterTodasReservas() => _real.ObterTodasReservas();

    public Reserva? ObterReservaPorId(string id) => _real.ObterReservaPorId(id);

    public Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida)
    {
        if (!DatasValidas(entrada, saida))
            return new Dictionary<string, int>();

        return _real.ObterDisponibilidadeCompleta(entrada, saida);
    }

    public Reserva? CriarReservaWeb(string nomeHospede, string tipoQuarto, DateTime entrada, DateTime saida, PacoteHospedagem pacote)
    {
        if (!DatasValidas(entrada, saida))
            return null;

        _logger.LogInformation("Criando reserva web para {Hospede}, quarto {Tipo}", nomeHospede, tipoQuarto);
        return _real.CriarReservaWeb(nomeHospede, tipoQuarto, entrada, saida, pacote);
    }

    private static bool DatasValidas(DateTime entrada, DateTime saida) =>
        entrada < saida && entrada.Date >= DateTime.Today.AddDays(-1);
}
