namespace SistemaHotelaria.Services.Observer;

/// <summary>
/// Observer Pattern: Concrete observer - Reception desk.
/// </summary>
public class Recepcao : IObserver
{
    public void Atualizar(string mensagem)
    {
        Console.WriteLine($"🛎️ [Recepção] {mensagem}");
    }
}
