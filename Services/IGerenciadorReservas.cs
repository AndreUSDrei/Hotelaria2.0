using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

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
    Reserva? ObterReservaPorId(string id);
    Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida);
    Reserva? CriarReservaWeb(string nomeHospede, string tipoQuarto, DateTime entrada, DateTime saida, PacoteHospedagem pacote, string metodoPagamento = "Pix");
}
