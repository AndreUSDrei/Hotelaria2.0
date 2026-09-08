namespace SistemaHotelaria.Services.Observer;

/// <summary>
/// Observer Pattern: Concrete observer - Email service.
/// </summary>
public class ServicoEmail : IObserver
{
    public void Atualizar(string mensagem)
    {
        Console.WriteLine($"📧 [E-mail] {mensagem}");
    }
}
