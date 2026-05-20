using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Notifications;

/// <summary>
/// Adapter: converte o modelo Reserva (e pacote) em mensagens para INotificacaoReserva.
/// </summary>
public class ReservaNotificacaoAdapter
{
    public void ExibirDetalhes(Reserva reserva, INotificacaoReserva notificacao)
    {
        notificacao.Informar($"📋 Reserva #{reserva.Id}");
        notificacao.Informar($"   Hóspede: {reserva.HospedeNome}");
        notificacao.Informar($"   Quarto: {reserva.TipoQuarto}");
        notificacao.Informar($"   Período: {reserva.DataEntrada:dd/MM/yyyy} a {reserva.DataSaida:dd/MM/yyyy}");
        notificacao.Informar(
            $"   Status: {(reserva.CheckInRealizado ? "✅ Check-in" : "⏳ Aguardando")} / {(reserva.CheckOutRealizado ? "✅ Check-out" : "⏳ Aguardando")}");
        notificacao.Informar($"   Valor Total: R$ {reserva.ValorTotal:N2}");

        if (reserva.Pacote == null)
            return;

        notificacao.Informar($"   📦 Pacote: {reserva.Pacote.Nome}");
        notificacao.Informar($"   {reserva.Pacote.Descricao}");
        if (reserva.Pacote.DescontoPercentual > 0)
            notificacao.Informar($"   🏷️  Desconto: {reserva.Pacote.DescontoPercentual}%");
    }
}
