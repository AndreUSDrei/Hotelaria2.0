using Microsoft.AspNetCore.Mvc;
using SistemaHotelaria.Models;
using SistemaHotelaria.Services.Notifications;
using SistemaHotelaria.Services.Observer;
using SistemaHotelaria.Services.Strategy;

namespace SistemaHotelaria.Controllers;

/// <summary>
/// Simplified Reservations Controller demonstrating Observer, Strategy, and Composite patterns.
/// </summary>
public class ReservasController : Controller
{
    private static readonly List<Reserva> _reservas = new();
    private static readonly List<IObserver> _observadores = new();
    private static readonly INotificacaoReserva _notificacaoComposite = new CompositeNotificacaoAdapter();

    static ReservasController()
    {
        // Initialize observers
        _observadores.Add(new Recepcao());
        _observadores.Add(new ServicoEmail());
        _observadores.Add(new ServicoLimpeza());

        // Initialize composite notifications
        var composite = (CompositeNotificacaoAdapter)_notificacaoComposite;
        composite.Adicionar(new ConsoleNotificacaoAdapter());
        composite.Adicionar(new WebNotificacaoAdapter());
    }

    public IActionResult Index()
    {
        return View(_reservas);
    }

    public IActionResult Criar()
    {
        ViewBag.MetodosPagamento = new[] { "Pix", "Cartão", "Boleto" };
        ViewBag.TiposQuarto = TipoQuarto.ObterTodosTipos();
        return View();
    }

    [HttpPost]
    public IActionResult Criar(string hospedeNome, string tipoQuarto, DateTime dataEntrada, 
                               DateTime dataSaida, string metodoPagamento)
    {
        // Calcular número de diárias
        var dias = (dataSaida - dataEntrada).Days;
        if (dias <= 0)
        {
            TempData["Erro"] = "A data de saída deve ser posterior à data de entrada.";
            return RedirectToAction(nameof(Criar));
        }

        // Calcular valor total baseado no tipo de quarto e número de diárias
        var precoDiario = TipoQuarto.ObterPrecoDiario(tipoQuarto);
        var valorTotal = precoDiario * dias;

        var reserva = new Reserva
        {
            HospedeNome = hospedeNome,
            TipoQuarto = tipoQuarto,
            DataEntrada = dataEntrada,
            DataSaida = dataSaida,
            ValorTotal = valorTotal
        };

        // Observer Pattern: Attach observers
        foreach (var obs in _observadores)
            reserva.Anexar(obs);

        // Strategy Pattern: Set payment strategy
        var estrategia = EstrategiaPagamentoFactory.Criar(metodoPagamento);
        reserva.DefinirEstrategiaPagamento(estrategia);

        // Process payment
        reserva.ProcessarPagamento(valorTotal);

        _reservas.Add(reserva);
        
        // Composite Pattern: Send notifications
        _notificacaoComposite.InformarSucesso($"Reserva #{reserva.Id} criada com sucesso!");

        TempData["Sucesso"] = $"Reserva #{reserva.Id} criada com sucesso via {metodoPagamento}";
        return RedirectToAction(nameof(Detalhes), new { id = reserva.Id });
    }

    public IActionResult Detalhes(string id)
    {
        var reserva = _reservas.FirstOrDefault(r => r.Id == id);
        if (reserva == null)
            return NotFound();

        return View(reserva);
    }

    [HttpPost]
    public IActionResult CheckIn(string id)
    {
        var reserva = _reservas.FirstOrDefault(r => r.Id == id);
        if (reserva == null)
            return NotFound();

        reserva.CheckIn();
        _notificacaoComposite.InformarSucesso($"Check-in realizado para reserva #{id}");
        
        TempData["Sucesso"] = "Check-in realizado com sucesso";
        return RedirectToAction(nameof(Detalhes), new { id });
    }

    [HttpPost]
    public IActionResult CheckOut(string id)
    {
        var reserva = _reservas.FirstOrDefault(r => r.Id == id);
        if (reserva == null)
            return NotFound();

        reserva.CheckOut();
        _notificacaoComposite.InformarSucesso($"Check-out realizado para reserva #{id}");
        
        TempData["Sucesso"] = "Check-out realizado com sucesso";
        return RedirectToAction(nameof(Detalhes), new { id });
    }
}
