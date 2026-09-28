namespace SistemaHotelaria.Services.Notifications;

/// <summary>
/// Adapter: contrato de notificação independente do canal (console, web, e-mail).
/// </summary>
public interface INotificacaoReserva
{
    void Informar(string mensagem);
    void InformarErro(string mensagem);
    void InformarSucesso(string mensagem);
}
