namespace SistemaHotelaria.Services.Observer;

/// <summary>
/// Observer Pattern: Observer interface that receives notifications from subject.
/// </summary>
public interface IObserver
{
    void Atualizar(string mensagem);
}
