namespace SistemaHotelaria.Prototype;

public interface IQuarto : ICloneable
{
    string Tipo { get; }
    string Descricao { get; set; }
    int Capacidade { get; set; }
    decimal PrecoBaseDiaria { get; set; }
    List<string> Comodidades { get; set; }

    new IQuarto Clone();
    void ExibirDetalhes();
}
