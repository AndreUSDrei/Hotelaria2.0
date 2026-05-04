namespace SistemaHotelaria.Prototype;

public class Suite : IQuarto
{
    public string Tipo => "Suíte Presidencial";
    public string Descricao { get; set; } = "Suíte presidencial com todos os serviços de luxo";
    public int Capacidade { get; set; } = 4;
    public decimal PrecoBaseDiaria { get; set; } = 800.00m;
    public List<string> Comodidades { get; set; } = new()
    {
        "Cama king-size premium",
        "Sala de estar separada",
        "Escritório privativo",
        "Cozinha compacta",
        "Banheira de hidromassagem dupla",
        "Sauna privativa",
        "Varanda gourmet",
        "Smart TV 65\" + Home Theater",
        "Bar exclusivo",
        "Serviço de mordomo",
        "Transfer aeroporto",
        "Wi-Fi premium"
    };

    public int NumeroQuartos { get; set; } = 2;
    public bool ServicoMordomo24h { get; set; } = true;

    public IQuarto Clone()
    {
        return new Suite
        {
            Descricao = this.Descricao,
            Capacidade = this.Capacidade,
            PrecoBaseDiaria = this.PrecoBaseDiaria,
            Comodidades = new List<string>(this.Comodidades),
            NumeroQuartos = this.NumeroQuartos,
            ServicoMordomo24h = this.ServicoMordomo24h
        };
    }

    object ICloneable.Clone() => Clone();

    public void ExibirDetalhes()
    {
        Console.WriteLine($"\n   👑 {Tipo}");
        Console.WriteLine($"   {Descricao}");
        Console.WriteLine($"   👥 Capacidade: {Capacidade} pessoas");
        Console.WriteLine($"   🚪 Ambientes: {NumeroQuartos} cômodos");
        Console.WriteLine($"   💰 Diária: R$ {PrecoBaseDiaria:N2}");
        Console.WriteLine($"   🎩 Mordomo 24h: {(ServicoMordomo24h ? "Sim" : "Não")}");
        Console.WriteLine($"   📦 Comodidades: {string.Join(", ", Comodidades)}");
    }
}
