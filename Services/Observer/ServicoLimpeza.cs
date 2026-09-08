namespace SistemaHotelaria.Services.Observer;

/// <summary>
/// Observer Pattern: Concrete observer - Cleaning service.
/// </summary>
public class ServicoLimpeza : IObserver
{
    public void Atualizar(string mensagem)
    {
        Console.WriteLine($"🧹 [Limpeza] {mensagem}");
    }
}
