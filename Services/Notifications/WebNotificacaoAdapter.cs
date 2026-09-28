namespace SistemaHotelaria.Services.Notifications;

/// <summary>
/// Adapter: canal web (sem saída no console; mensagens ficam na camada de apresentação via TempData).
/// </summary>
public class WebNotificacaoAdapter : INotificacaoReserva
{
    public void Informar(string mensagem) { }

    public void InformarErro(string mensagem) { }

    public void InformarSucesso(string mensagem) { }
}
