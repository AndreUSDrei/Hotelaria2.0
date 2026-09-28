namespace SistemaHotelaria.Prototype;

public class QuartoLuxo : IQuarto
{
    public string Tipo => "Luxo";
    public string Descricao { get; set; } = "Quarto com vista panorâmica e amenidades premium";
    public int Capacidade { get; set; } = 3;
    public decimal PrecoBaseDiaria { get; set; } = 350.00m;
    public List<string> Comodidades { get; set; } = new()
    {
        "Cama king-size",
        "Vista panorâmica",
        "Banheira de hidromassagem",
        "Smart TV 55\"",
        "Mini bar",
        "Cafeteira Nespresso",
        "Roupão e chinelos",
        "Wi-Fi premium"
    };

    public bool PossoVaranda { get; set; } = true;

    public IQuarto Clone()
    {
        return new QuartoLuxo
        {
            Descricao = this.Descricao,
            Capacidade = this.Capacidade,
            PrecoBaseDiaria = this.PrecoBaseDiaria,
            Comodidades = new List<string>(this.Comodidades),
            PossoVaranda = this.PossoVaranda
        };
    }

    object ICloneable.Clone() => Clone();

    public void ExibirDetalhes()
    {
        Console.WriteLine($"\n   🏨 Quarto {Tipo}");
        Console.WriteLine($"   {Descricao}");
        Console.WriteLine($"   👥 Capacidade: {Capacidade} pessoas");
        Console.WriteLine($"   💰 Diária: R$ {PrecoBaseDiaria:N2}");
        Console.WriteLine($"   🌅 Varanda: {(PossoVaranda ? "Sim" : "Não")}");
        Console.WriteLine($"   📦 Comodidades: {string.Join(", ", Comodidades)}");
    }
}
