namespace SistemaHotelaria.Services.Notifications;

/// <summary>
/// Adapter: adapta Console.WriteLine ao contrato INotificacaoReserva.
/// </summary>
public class ConsoleNotificacaoAdapter : INotificacaoReserva
{
    public void Informar(string mensagem) => Console.WriteLine(mensagem);

    public void InformarErro(string mensagem) => Console.WriteLine(mensagem);

    public void InformarSucesso(string mensagem) => Console.WriteLine(mensagem);
}
