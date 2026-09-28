namespace SistemaHotelaria.Services.Commands;

public interface ICommandFactory
{
    ICommand CreateCheckInCommand(string idReserva);
    ICommand CreateCheckOutCommand(string idReserva);
    ICommand CreateCancelarReservaCommand(string idReserva);
    ICommand CreateCriarReservaCommand(
        string nomeHospede,
        string tipoQuarto,
        DateTime entrada,
        DateTime saida,
        object pacote,
        string metodoPagamento = "Pix",
        string? numeroCartao = null,
        string? cvv = null);
}