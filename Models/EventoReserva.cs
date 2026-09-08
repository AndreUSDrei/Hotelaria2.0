namespace SistemaHotelaria.Models;

public class AcaoServico
{
    public string Servico { get; set; } = string.Empty;
    public string Icone { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}

public class EventoReserva
{
    public string Status { get; set; } = string.Empty;
    public DateTime Quando { get; set; }
    public List<AcaoServico> Acoes { get; set; } = new();
}
