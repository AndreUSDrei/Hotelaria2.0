namespace SistemaHotelaria.Services.Observer;

public interface ISubject
{
    void Anexar(IObserver observador);
    void Desanexar(IObserver observador);
    void Notificar();
}
