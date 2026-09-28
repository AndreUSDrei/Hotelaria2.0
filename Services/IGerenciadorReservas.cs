using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;
using SistemaHotelaria.Services.Iterator;

namespace SistemaHotelaria.Services;

public interface IGerenciadorReservas
{
    bool QuartoDisponivel(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida);
    int ObterQuantidadeQuartosDisponiveis(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida);
    Reserva? CriarReserva(string nomeHospede, IQuarto quartoBase, IPacoteHospedagemBuilder builder,
        HotelDirector director, DateTime entrada, DateTime saida);
    bool RealizarCheckIn(string idReserva);
    bool RealizarCheckOut(string idReserva);
    void ListarReservas();
    void ExibirDisponibilidade(DateTime dataEntrada, DateTime dataSaida);
    List<Reserva> ObterTodasReservas();
    List<Reserva> ObterReservasFiltradas(FiltroReservas filtro);
    Reserva? ObterReservaPorId(string id);
    Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida);
    ResultadoCriacaoReserva CriarReservaWeb(string nomeHospede, string tipoQuarto, DateTime entrada, DateTime saida, PacoteHospedagem pacote,
        string metodoPagamento = "Pix", string? numeroCartao = null, string? cvv = null);
    bool CancelarReserva(string idReserva);
    void AtualizarReserva(Reserva reserva);
}
