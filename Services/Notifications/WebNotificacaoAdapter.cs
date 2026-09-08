namespace SistemaHotelaria.Services.Notifications;

/// <summary>
/// Composite Pattern: Leaf component - Web notification.
/// </summary>
public class WebNotificacaoAdapter : INotificacaoReserva
{
    public void Informar(string mensagem) => Console.WriteLine($"[Web] {mensagem}");

    public void InformarErro(string mensagem) => Console.WriteLine($"[Web] ERRO: {mensagem}");

    public void InformarSucesso(string mensagem) => Console.WriteLine($"[Web] SUCESSO: {mensagem}");
}
