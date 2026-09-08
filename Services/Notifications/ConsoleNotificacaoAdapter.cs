namespace SistemaHotelaria.Services.Notifications;

/// <summary>
/// Composite Pattern: Leaf component - Console notification.
/// </summary>
public class ConsoleNotificacaoAdapter : INotificacaoReserva
{
    public void Informar(string mensagem) => Console.WriteLine($"[Console] {mensagem}");

    public void InformarErro(string mensagem) => Console.WriteLine($"[Console] ERRO: {mensagem}");

    public void InformarSucesso(string mensagem) => Console.WriteLine($"[Console] SUCESSO: {mensagem}");
}
