namespace SistemaHotelaria.Services.Notifications;

/// <summary>
/// Composite Pattern: Composite component that manages multiple notification channels.
/// </summary>
public class CompositeNotificacaoAdapter : INotificacaoReserva
{
    private readonly List<INotificacaoReserva> _adapters = new();

    public void Adicionar(INotificacaoReserva adapter) => _adapters.Add(adapter);

    public void Remover(INotificacaoReserva adapter) => _adapters.Remove(adapter);

    public void Informar(string mensagem)
    {
        Console.WriteLine("[Composite] Enviando para todos os canais:");
        foreach (var adapter in _adapters)
            adapter.Informar(mensagem);
    }

    public void InformarErro(string mensagem)
    {
        Console.WriteLine("[Composite] Enviando erro para todos os canais:");
        foreach (var adapter in _adapters)
            adapter.InformarErro(mensagem);
    }

    public void InformarSucesso(string mensagem)
    {
        Console.WriteLine("[Composite] Enviando sucesso para todos os canais:");
        foreach (var adapter in _adapters)
            adapter.InformarSucesso(mensagem);
    }
}
