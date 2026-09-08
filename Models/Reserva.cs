using SistemaHotelaria.Services.Observer;
using SistemaHotelaria.Services.Strategy;

namespace SistemaHotelaria.Models;

/// <summary>
/// Simplified Reservation model demonstrating Observer and Strategy patterns.
/// </summary>
public class Reserva : ISubject
{
    private readonly List<IObserver> _observadores = new();
    private IEstrategiaPagamento? _estrategiaPagamento;

    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();
    public string HospedeNome { get; set; } = string.Empty;
    public string TipoQuarto { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Confirmada";
    public string MetodoPagamento { get; set; } = string.Empty;
    public string TransacaoId { get; set; } = string.Empty;

    // Observer Pattern: Subject implementation
    public void Anexar(IObserver observador)
    {
        if (!_observadores.Contains(observador))
            _observadores.Add(observador);
    }

    public void Desanexar(IObserver observador) => _observadores.Remove(observador);

    public void Notificar(string mensagem)
    {
        foreach (var observador in _observadores)
            observador.Atualizar(mensagem);
    }

    // Strategy Pattern: Payment strategy
    public void DefinirEstrategiaPagamento(IEstrategiaPagamento estrategia) =>
        _estrategiaPagamento = estrategia;

    public ResultadoPagamento? ProcessarPagamento(decimal valor)
    {
        if (_estrategiaPagamento == null)
            return null;

        var resultado = _estrategiaPagamento.Pagar(valor);
        if (resultado.Sucesso)
        {
            MetodoPagamento = resultado.Metodo;
            TransacaoId = resultado.TransacaoId;
            Status = "Paga";
            Notificar($"Reserva #{Id} de {HospedeNome} foi paga via {resultado.Metodo}");
        }
        return resultado;
    }

    public void CheckIn()
    {
        Status = "Check-in";
        Notificar($"{HospedeNome} fez check-in no quarto {TipoQuarto}");
    }

    public void CheckOut()
    {
        Status = "Check-out";
        Notificar($"{HospedeNome} fez check-out do quarto {TipoQuarto}");
    }

    public void ExibirDetalhes()
    {
        Console.WriteLine($"📋 Reserva #{Id}");
        Console.WriteLine($"   Hóspede: {HospedeNome}");
        Console.WriteLine($"   Quarto: {TipoQuarto}");
        Console.WriteLine($"   Período: {DataEntrada:dd/MM/yyyy} a {DataSaida:dd/MM/yyyy}");
        Console.WriteLine($"   Status: {Status}");
        Console.WriteLine($"   Valor Total: R$ {ValorTotal:N2}");
        if (!string.IsNullOrEmpty(MetodoPagamento))
            Console.WriteLine($"   Pagamento: {MetodoPagamento} ({TransacaoId})");
    }
}
