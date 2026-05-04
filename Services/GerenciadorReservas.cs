using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Services;

public class GerenciadorReservas
{
    private readonly List<Reserva> _reservasAtivas = new();
    private int _contadorQuartos = 100;

    private readonly Dictionary<string, int> _quartosPorTipo = new()
    {
        { "Standard", 20 },
        { "Luxo", 10 },
        { "Suíte Presidencial", 3 }
    };

    public bool QuartoDisponivel(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        var reservasConflitantes = _reservasAtivas.Count(r =>
            r.TipoQuarto == tipoQuarto &&
            !r.CheckOutRealizado &&
            r.DataEntrada < dataSaida &&
            r.DataSaida > dataEntrada);

        return reservasConflitantes < _quartosPorTipo.GetValueOrDefault(tipoQuarto, 0);
    }

    public int ObterQuantidadeQuartosDisponiveis(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        var reservasConflitantes = _reservasAtivas.Count(r =>
            r.TipoQuarto == tipoQuarto &&
            !r.CheckOutRealizado &&
            r.DataEntrada < dataSaida &&
            r.DataSaida > dataEntrada);

        int total = _quartosPorTipo.GetValueOrDefault(tipoQuarto, 0);
        return total - reservasConflitantes;
    }

    public Reserva? CriarReserva(string nomeHospede, IQuarto quartoBase, IPacoteHospedagemBuilder builder, 
                                 HotelDirector director, DateTime entrada, DateTime saida)
    {
        if (!QuartoDisponivel(quartoBase.Tipo, entrada, saida))
        {
            Console.WriteLine($"\n❌ Não há quartos {quartoBase.Tipo} disponíveis para o período solicitado.");
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

        _reservasAtivas.Add(reserva);
        Console.WriteLine($"\n✅ Reserva #{reserva.Id} criada com sucesso!");
        
        return reserva;
    }

    public bool RealizarCheckIn(string idReserva)
    {
        var reserva = _reservasAtivas.FirstOrDefault(r => r.Id == idReserva);
        if (reserva == null)
        {
            Console.WriteLine($"❌ Reserva {idReserva} não encontrada.");
            return false;
        }

        if (reserva.CheckInRealizado)
        {
            Console.WriteLine($"⚠️ Check-in já realizado para reserva {idReserva}.");
            return false;
        }

        reserva.CheckInRealizado = true;
        int numeroQuarto = ++_contadorQuartos;
        Console.WriteLine($"\n🏨 Check-in realizado! Quarto atribuído: {numeroQuarto}");
        return true;
    }

    public bool RealizarCheckOut(string idReserva)
    {
        var reserva = _reservasAtivas.FirstOrDefault(r => r.Id == idReserva);
        if (reserva == null)
        {
            Console.WriteLine($"❌ Reserva {idReserva} não encontrada.");
            return false;
        }

        if (!reserva.CheckInRealizado)
        {
            Console.WriteLine($"⚠️ Check-in não realizado para reserva {idReserva}.");
            return false;
        }

        if (reserva.CheckOutRealizado)
        {
            Console.WriteLine($"⚠️ Check-out já realizado para reserva {idReserva}.");
            return false;
        }

        reserva.CheckOutRealizado = true;
        Console.WriteLine($"\n👋 Check-out realizado! Obrigado pela estadia.");
        return true;
    }

    public void ListarReservas()
    {
        Console.WriteLine("\n═══════════════════════════════════════════════════════════");
        Console.WriteLine("📋 RESERVAS ATIVAS");
        Console.WriteLine("═══════════════════════════════════════════════════════════");

        if (!_reservasAtivas.Any())
        {
            Console.WriteLine("Nenhuma reserva ativa.");
            return;
        }

        foreach (var reserva in _reservasAtivas)
        {
            reserva.ExibirDetalhes();
            Console.WriteLine("───────────────────────────────────────────────────────────");
        }
    }

    public void ExibirDisponibilidade(DateTime dataEntrada, DateTime dataSaida)
    {
        Console.WriteLine("\n═══════════════════════════════════════════════════════════");
        Console.WriteLine($"📅 Disponibilidade: {dataEntrada:dd/MM/yyyy} a {dataSaida:dd/MM/yyyy}");
        Console.WriteLine("═══════════════════════════════════════════════════════════");

        foreach (var tipo in _quartosPorTipo.Keys)
        {
            int disponiveis = ObterQuantidadeQuartosDisponiveis(tipo, dataEntrada, dataSaida);
            int total = _quartosPorTipo[tipo];
            Console.WriteLine($"   {tipo}: {disponiveis}/{total} disponíveis");
        }
    }

    // Métodos para Web
    public List<Reserva> ObterTodasReservas() => _reservasAtivas.ToList();

    public Reserva? ObterReservaPorId(string id) => _reservasAtivas.FirstOrDefault(r => r.Id == id);

    public Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida)
    {
        var disponibilidade = new Dictionary<string, int>();
        foreach (var tipo in _quartosPorTipo.Keys)
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

        _reservasAtivas.Add(reserva);
        return reserva;
    }
}
