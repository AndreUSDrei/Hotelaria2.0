namespace SistemaHotelaria.Prototype;

public class QuartoStandard : IQuarto
{
    public string Tipo => "Standard";
    public string Descricao { get; set; } = "Quarto básico com comodidades essenciais";
    public int Capacidade { get; set; } = 2;
    public decimal PrecoBaseDiaria { get; set; } = 150.00m;
    public List<string> Comodidades { get; set; } = new()
    {
        "Cama de casal",
        "Banheiro privativo",
        "TV a cabo",
        "Ar-condicionado",
        "Wi-Fi"
    };

    public IQuarto Clone()
    {
        return new QuartoStandard
        {
            Descricao = this.Descricao,
            Capacidade = this.Capacidade,
            PrecoBaseDiaria = this.PrecoBaseDiaria,
            Comodidades = new List<string>(this.Comodidades)
        };
    }

    object ICloneable.Clone() => Clone();

    public void ExibirDetalhes()
    {
        Console.WriteLine($"\n   🏨 Quarto {Tipo}");
        Console.WriteLine($"   {Descricao}");
        Console.WriteLine($"   👥 Capacidade: {Capacidade} pessoas");
        Console.WriteLine($"   💰 Diária: R$ {PrecoBaseDiaria:N2}");
        Console.WriteLine($"   📦 Comodidades: {string.Join(", ", Comodidades)}");
    }
}
