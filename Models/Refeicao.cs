namespace SistemaHotelaria.Models;

public class Refeicao
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Horario { get; set; } = string.Empty;
    public decimal Preco { get; set; }

    public void Exibir()
    {
        Console.WriteLine($"      🍽️  {Nome} - {Descricao} ({Horario})");
    }
}
