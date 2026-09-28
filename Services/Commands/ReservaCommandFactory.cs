using SistemaHotelaria.Builder;
using SistemaHotelaria.Services;

namespace SistemaHotelaria.Services.Commands;

public class ReservaCommandFactory : ICommandFactory
{
    private readonly IGerenciadorReservas _gerenciadorReservas;

    public ReservaCommandFactory(IGerenciadorReservas gerenciadorReservas)
    {
        _gerenciadorReservas = gerenciadorReservas;
    }

    public ICommand CreateCheckInCommand(string idReserva)
    {
        return new CheckInCommand(_gerenciadorReservas, idReserva);
    }

    public ICommand CreateCheckOutCommand(string idReserva)
    {
        return new CheckOutCommand(_gerenciadorReservas, idReserva);
    }

    public ICommand CreateCancelarReservaCommand(string idReserva)
    {
        return new CancelarReservaCommand(_gerenciadorReservas, idReserva);
    }

    public ICommand CreateCriarReservaCommand(
        string nomeHospede,
        string tipoQuarto,
        DateTime entrada,
        DateTime saida,
        object pacote,
        string metodoPagamento = "Pix",
        string? numeroCartao = null,
        string? cvv = null)
    {
        if (pacote is PacoteHospedagem pacoteHospedagem)
        {
            return new CriarReservaCommand(
                _gerenciadorReservas,
                nomeHospede,
                tipoQuarto,
                entrada,
                saida,
                pacoteHospedagem,
                metodoPagamento,
                numeroCartao,
                cvv);
        }

        throw new ArgumentException("Pacote inválido", nameof(pacote));
    }
}