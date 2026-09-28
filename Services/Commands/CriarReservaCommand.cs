using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;
using SistemaHotelaria.Services;

namespace SistemaHotelaria.Services.Commands;

public class CriarReservaCommand : ICommand
{
    private readonly IGerenciadorReservas _gerenciadorReservas;
    private readonly string _nomeHospede;
    private readonly string _tipoQuarto;
    private readonly DateTime _entrada;
    private readonly DateTime _saida;
    private readonly PacoteHospedagem _pacote;
    private readonly string _metodoPagamento;
    private readonly string? _numeroCartao;
    private readonly string? _cvv;
    private Reserva? _reservaCriada;
    private bool _executado;

    public CriarReservaCommand(
        IGerenciadorReservas gerenciadorReservas,
        string nomeHospede,
        string tipoQuarto,
        DateTime entrada,
        DateTime saida,
        PacoteHospedagem pacote,
        string metodoPagamento = "Pix",
        string? numeroCartao = null,
        string? cvv = null)
    {
        _gerenciadorReservas = gerenciadorReservas;
        _nomeHospede = nomeHospede;
        _tipoQuarto = tipoQuarto;
        _entrada = entrada;
        _saida = saida;
        _pacote = pacote;
        _metodoPagamento = metodoPagamento;
        _numeroCartao = numeroCartao;
        _cvv = cvv;
        _executado = false;
    }

    public void Execute()
    {
        if (!_executado)
        {
            var resultado = _gerenciadorReservas.CriarReservaWeb(
                _nomeHospede,
                _tipoQuarto,
                _entrada,
                _saida,
                _pacote,
                _metodoPagamento,
                _numeroCartao,
                _cvv);
            _reservaCriada = resultado.Reserva;
            _executado = resultado.Sucesso;
        }
    }

    public void Undo()
    {
        if (_executado && _reservaCriada != null)
        {
            var reserva = _gerenciadorReservas.ObterReservaPorId(_reservaCriada.Id);
            if (reserva != null && reserva.Status == "Confirmada")
            {
                _gerenciadorReservas.RealizarCheckOut(_reservaCriada.Id);
                _executado = false;
            }
        }
    }

    public Reserva? GetReservaCriada() => _reservaCriada;
}