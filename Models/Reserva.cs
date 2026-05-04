using SistemaHotelaria.Builder;

namespace SistemaHotelaria.Models;

public class Reserva
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();
    public string HospedeNome { get; set; } = string.Empty;
    public string TipoQuarto { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public PacoteHospedagem? Pacote { get; set; }
    public decimal ValorTotal { get; set; }
    public bool CheckInRealizado { get; set; }
    public bool CheckOutRealizado { get; set; }

    public void ExibirDetalhes()
    {
        Console.WriteLine($"📋 Reserva #{Id}");
        Console.WriteLine($"   Hóspede: {HospedeNome}");
        Console.WriteLine($"   Período: {DataEntrada:dd/MM/yyyy} a {DataSaida:dd/MM/yyyy}");
        Console.WriteLine($"   Status: {(CheckInRealizado ? "✅ Check-in" : "⏳ Aguardando")} / {(CheckOutRealizado ? "✅ Check-out" : "⏳ Aguardando")}");
        Console.WriteLine($"   Valor Total: R$ {ValorTotal:N2}");
        Pacote?.ExibirDetalhes();
    }
}
