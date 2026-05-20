using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;
using SistemaHotelaria.Services.Notifications;
using SistemaHotelaria.Services.Persistence;

namespace SistemaHotelaria.Services;

public class GerenciadorReservas : IGerenciadorReservas
{
    private readonly IReservaRepository _repositorio;
    private readonly INotificacaoReserva _notificacao;
    private readonly IInventarioQuartos _inventario;
    private readonly ReservaNotificacaoAdapter _reservaNotificacao;
    private int _contadorQuartos = 100;

    public GerenciadorReservas(
        IReservaRepository repositorio,
        INotificacaoReserva notificacao,
        IInventarioQuartos inventario,
        ReservaNotificacaoAdapter reservaNotificacao)
    {
        _repositorio = repositorio;
        _notificacao = notificacao;
        _inventario = inventario;
        _reservaNotificacao = reservaNotificacao;
    }

    private IReadOnlyDictionary<string, int> QuartosPorTipo => _inventario.ObterCapacidadePorTipo();

    public bool QuartoDisponivel(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        var conflitos = _repositorio.ContarConflitos(tipoQuarto, dataEntrada, dataSaida);
        return conflitos < QuartosPorTipo.GetValueOrDefault(tipoQuarto, 0);
    }

    public int ObterQuantidadeQuartosDisponiveis(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        var conflitos = _repositorio.ContarConflitos(tipoQuarto, dataEntrada, dataSaida);
        int total = QuartosPorTipo.GetValueOrDefault(tipoQuarto, 0);
        return total - conflitos;
    }

    public Reserva? CriarReserva(string nomeHospede, IQuarto quartoBase, IPacoteHospedagemBuilder builder,
                                 HotelDirector director, DateTime entrada, DateTime saida)
    {
        if (!QuartoDisponivel(quartoBase.Tipo, entrada, saida))
        {
            _notificacao.InformarErro($"\n❌ Não há quartos {quartoBase.Tipo} disponíveis para o período solicitado.");
            return null;
        }

        var pacote = director.ObterPacote();
        int dias = (saida - entrada).Days;

        var reserva = new Reserva
        {
            HospedeNome = nomeHospede,
            TipoQuarto = quartoBase.Tipo,
            DataEntrada = entrada,
            DataSaida = saida,
            Pacote = pacote,
            ValorTotal = pacote.CalcularValorTotal(dias)
        };

        _repositorio.Adicionar(reserva);
        _notificacao.InformarSucesso($"\n✅ Reserva #{reserva.Id} criada com sucesso!");

        return reserva;
    }

    public bool RealizarCheckIn(string idReserva)
    {
        var reserva = _repositorio.ObterPorId(idReserva);
        if (reserva == null)
        {
            _notificacao.InformarErro($"❌ Reserva {idReserva} não encontrada.");
            return false;
        }

        if (reserva.CheckInRealizado)
        {
            _notificacao.Informar($"⚠️ Check-in já realizado para reserva {idReserva}.");
            return false;
        }

        reserva.CheckInRealizado = true;
        int numeroQuarto = ++_contadorQuartos;
        _notificacao.InformarSucesso($"\n🏨 Check-in realizado! Quarto atribuído: {numeroQuarto}");
        return true;
    }

    public bool RealizarCheckOut(string idReserva)
    {
        var reserva = _repositorio.ObterPorId(idReserva);
        if (reserva == null)
        {
            _notificacao.InformarErro($"❌ Reserva {idReserva} não encontrada.");
            return false;
        }

        if (!reserva.CheckInRealizado)
        {
            _notificacao.Informar($"⚠️ Check-in não realizado para reserva {idReserva}.");
            return false;
        }

        if (reserva.CheckOutRealizado)
        {
            _notificacao.Informar($"⚠️ Check-out já realizado para reserva {idReserva}.");
            return false;
        }

        reserva.CheckOutRealizado = true;
        _notificacao.InformarSucesso($"\n👋 Check-out realizado! Obrigado pela estadia.");
        return true;
    }

    public void ListarReservas()
    {
        _notificacao.Informar("\n═══════════════════════════════════════════════════════════");
        _notificacao.Informar("📋 RESERVAS ATIVAS");
        _notificacao.Informar("═══════════════════════════════════════════════════════════");

        var reservas = _repositorio.ObterTodas();
        if (!reservas.Any())
        {
            _notificacao.Informar("Nenhuma reserva ativa.");
            return;
        }

        foreach (var reserva in reservas)
        {
            _reservaNotificacao.ExibirDetalhes(reserva, _notificacao);
            _notificacao.Informar("───────────────────────────────────────────────────────────");
        }
    }

    public void ExibirDisponibilidade(DateTime dataEntrada, DateTime dataSaida)
    {
        _notificacao.Informar("\n═══════════════════════════════════════════════════════════");
        _notificacao.Informar($"📅 Disponibilidade: {dataEntrada:dd/MM/yyyy} a {dataSaida:dd/MM/yyyy}");
        _notificacao.Informar("═══════════════════════════════════════════════════════════");

        foreach (var tipo in QuartosPorTipo.Keys)
        {
            int disponiveis = ObterQuantidadeQuartosDisponiveis(tipo, dataEntrada, dataSaida);
            int total = QuartosPorTipo[tipo];
            _notificacao.Informar($"   {tipo}: {disponiveis}/{total} disponíveis");
        }
    }

    public List<Reserva> ObterTodasReservas() => _repositorio.ObterTodas().ToList();

    public Reserva? ObterReservaPorId(string id) => _repositorio.ObterPorId(id);

    public Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida)
    {
        var disponibilidade = new Dictionary<string, int>();
        foreach (var tipo in QuartosPorTipo.Keys)
        {
            disponibilidade[tipo] = ObterQuantidadeQuartosDisponiveis(tipo, entrada, saida);
        }
        return disponibilidade;
    }

    public Reserva? CriarReservaWeb(string nomeHospede, string tipoQuarto, DateTime entrada, DateTime saida, PacoteHospedagem pacote)
    {
        if (!QuartoDisponivel(tipoQuarto, entrada, saida))
            return null;

        int dias = (saida - entrada).Days;
        var reserva = new Reserva
        {
            HospedeNome = nomeHospede,
            TipoQuarto = tipoQuarto,
            DataEntrada = entrada,
            DataSaida = saida,
            Pacote = pacote,
            ValorTotal = pacote.CalcularValorTotal(dias)
        };

        _repositorio.Adicionar(reserva);
        return reserva;
    }
}
