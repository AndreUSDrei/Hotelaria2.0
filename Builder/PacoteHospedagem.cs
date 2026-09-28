using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Builder;

public class PacoteHospedagem
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public IQuarto? Quarto { get; set; }
    public List<Refeicao> Refeicoes { get; set; } = new();
    public List<ServicoAdicional> Servicos { get; set; } = new();
    public decimal DescontoPercentual { get; set; }

    public decimal CalcularValorTotal(int dias)
    {
        if (Quarto == null) return 0;

        decimal valorDiarias = Quarto.PrecoBaseDiaria * dias;
        decimal valorRefeicoes = Refeicoes.Sum(r => r.Preco) * dias;
        decimal valorServicos = Servicos.Sum(s => s.Preco);

        decimal subtotal = valorDiarias + valorRefeicoes + valorServicos;
        decimal desconto = subtotal * (DescontoPercentual / 100);

        return subtotal - desconto;
    }

    public void ExibirDetalhes()
    {
        Console.WriteLine($"   📦 Pacote: {Nome}");
        Console.WriteLine($"   {Descricao}");
        if (DescontoPercentual > 0)
            Console.WriteLine($"   🏷️  Desconto: {DescontoPercentual}%");

        if (Refeicoes.Any())
        {
            Console.WriteLine("   🍽️  Refeições incluídas:");
            foreach (var r in Refeicoes) r.Exibir();
        }

        if (Servicos.Any())
        {
            Console.WriteLine("   ✨ Serviços adicionais:");
            foreach (var s in Servicos) s.Exibir();
        }
    }
}
