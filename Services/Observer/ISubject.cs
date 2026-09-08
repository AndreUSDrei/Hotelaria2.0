namespace SistemaHotelaria.Services.Observer;

/// <summary>
/// Observer Pattern: Subject interface that notifies observers about changes.
/// </summary>
public interface ISubject
{
    void Anexar(IObserver observador);
    void Desanexar(IObserver observador);
    void Notificar(string mensagem);
}
