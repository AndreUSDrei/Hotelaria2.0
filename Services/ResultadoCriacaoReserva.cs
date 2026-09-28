using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services;

/// <summary>
/// Resultado da criação de reserva (sucesso ou mensagem de erro para a View).
/// </summary>
public class ResultadoCriacaoReserva
{
    public bool Sucesso => Reserva != null && string.IsNullOrEmpty(MensagemErro);
    public Reserva? Reserva { get; init; }
    public string? MensagemErro { get; init; }

    public static ResultadoCriacaoReserva Ok(Reserva reserva) =>
        new() { Reserva = reserva };

    public static ResultadoCriacaoReserva Falha(string mensagem) =>
        new() { MensagemErro = mensagem };
}
