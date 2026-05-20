namespace SistemaHotelaria.Services.Notifications;

/// <summary>
/// Adapter composto: encaminha notificações para Console e Web (usa todos os adapters de canal).
/// </summary>
public class CompositeNotificacaoAdapter : INotificacaoReserva
{
    private readonly IReadOnlyList<INotificacaoReserva> _adapters;

    public CompositeNotificacaoAdapter(params INotificacaoReserva[] adapters)
    {
        _adapters = adapters;
    }

    public void Informar(string mensagem)
    {
        foreach (var adapter in _adapters)
            adapter.Informar(mensagem);
    }

    public void InformarErro(string mensagem)
    {
        foreach (var adapter in _adapters)
            adapter.InformarErro(mensagem);
    }

    public void InformarSucesso(string mensagem)
    {
        foreach (var adapter in _adapters)
            adapter.InformarSucesso(mensagem);
    }
}
