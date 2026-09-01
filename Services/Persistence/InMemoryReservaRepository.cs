using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Persistence;

/// <summary>
/// Adapter: adapta o armazenamento em memória (List) à interface IReservaRepository.
/// </summary>
public class InMemoryReservaRepository : IReservaRepository
{
    private readonly List<Reserva> _reservas = new();

    public IReadOnlyList<Reserva> ObterTodas() => _reservas;

    public Reserva? ObterPorId(string id) => _reservas.FirstOrDefault(r => r.Id == id);

    public void Adicionar(Reserva reserva) => _reservas.Add(reserva);

    public void Atualizar(Reserva reserva)
    {
        var existente = _reservas.FirstOrDefault(r => r.Id == reserva.Id);
        if (existente == null)
            return;

        existente.HospedeNome = reserva.HospedeNome;
        existente.TipoQuarto = reserva.TipoQuarto;
        existente.DataEntrada = reserva.DataEntrada;
        existente.DataSaida = reserva.DataSaida;
        existente.ValorTotal = reserva.ValorTotal;
        existente.AtualizarEstado(reserva.EstadoAtual);
        existente.Pacote = reserva.Pacote;
        existente.MetodoPagamento = reserva.MetodoPagamento;
        existente.PagamentoTransacaoId = reserva.PagamentoTransacaoId;
        existente.PagamentoComprovante = reserva.PagamentoComprovante;
        existente.Eventos = reserva.Eventos;
    }

    public int ContarConflitos(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        return _reservas.Count(r => 
            r.TipoQuarto == tipoQuarto && 
            r.Status != "Check-out" && r.Status != "Cancelada" &&
            r.DataEntrada < dataSaida && 
            r.DataSaida > dataEntrada);
    }
}
