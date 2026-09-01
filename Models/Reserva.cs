using SistemaHotelaria.Builder;
using SistemaHotelaria.Services.Observer;
using SistemaHotelaria.Services.Strategy;
using SistemaHotelaria.Models.States;

namespace SistemaHotelaria.Models;

public class Reserva : ISubject
{
    private readonly List<IObserver> _observadores = new();
    private readonly List<AcaoServico> _acoesPendentes = new();
    private IEstrategiaPagamento? _estrategiaPagamento;

    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();
    public string HospedeNome { get; set; } = string.Empty;
    public string TipoQuarto { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public PacoteHospedagem? Pacote { get; set; }
    public decimal ValorTotal { get; set; }
    
    public IEstadoReserva EstadoAtual { get; set; } = new EstadoConfirmada();
    public string Status => EstadoAtual.Nome;

    public string MetodoPagamento { get; set; } = "Pix";
    public string PagamentoTransacaoId { get; set; } = string.Empty;
    public string PagamentoComprovante { get; set; } = string.Empty;
    public List<EventoReserva> Eventos { get; set; } = new();

    public void Anexar(IObserver observador)
    {
        if (!_observadores.Contains(observador))
            _observadores.Add(observador);
    }

    public void Desanexar(IObserver observador) => _observadores.Remove(observador);

    public void Notificar()
    {
        foreach (var observador in _observadores)
            observador.Atualizar(this);
    }

    // Delegações para o State
    public void CheckIn() => EstadoAtual.CheckIn(this);
    public void CheckOut() => EstadoAtual.CheckOut(this);
    public void Cancelar() => EstadoAtual.Cancelar(this);

    // Chamado pelas classes de Estado
    public void AtualizarEstado(IEstadoReserva novoEstado)
    {
        EstadoAtual = novoEstado;

        _acoesPendentes.Clear();
        Notificar(); // Integração perfeita com o Observer

        Eventos.Add(new EventoReserva
        {
            Status = novoEstado.Nome,
            Quando = DateTime.Now,
            Acoes = _acoesPendentes.Select(a => new AcaoServico
            {
                Servico = a.Servico,
                Icone = a.Icone,
                Descricao = a.Descricao
            }).ToList()
        });
    }

    // Usado para forçar o estado ao carregar do EF Core
    public void DefinirEstadoInicial(IEstadoReserva estado)
    {
        EstadoAtual = estado;
    }

    public void RegistrarAcao(string servico, string icone, string descricao)
    {
        _acoesPendentes.Add(new AcaoServico
        {
            Servico = servico,
            Icone = icone,
            Descricao = descricao
        });
    }

    public void DefinirEstrategiaPagamento(IEstrategiaPagamento estrategia) =>
        _estrategiaPagamento = estrategia;

    public ResultadoPagamento? ProcessarPagamento(decimal valor)
    {
        if (_estrategiaPagamento == null)
            return null;

        var resultado = _estrategiaPagamento.Pagar(valor);
        if (!resultado.Sucesso)
            return resultado;

        MetodoPagamento = resultado.Metodo;
        PagamentoTransacaoId = resultado.TransacaoId;
        PagamentoComprovante = resultado.ComprovanteTexto;
        return resultado;
    }

    public void ExibirDetalhes()
    {
        Console.WriteLine($"📋 Reserva #{Id}");
        Console.WriteLine($"   Hóspede: {HospedeNome}");
        Console.WriteLine($"   Período: {DataEntrada:dd/MM/yyyy} a {DataSaida:dd/MM/yyyy}");
        Console.WriteLine($"   Status: {Status}");
        Console.WriteLine($"   Valor Total: R$ {ValorTotal:N2}");
        Console.WriteLine($"   Pagamento: {MetodoPagamento} ({PagamentoTransacaoId})");
        Pacote?.ExibirDetalhes();
    }
}
