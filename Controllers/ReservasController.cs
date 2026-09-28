using Microsoft.AspNetCore.Mvc;
using SistemaHotelaria.Services.Commands;
using SistemaHotelaria.Services.Facade;
using SistemaHotelaria.Services.Iterator;

namespace SistemaHotelaria.Controllers;

public class ReservasController : Controller
{
    private readonly IReservaFacade _reservaFacade;
    private readonly ICommandInvoker _commandInvoker;
    private readonly ICommandFactory _commandFactory;

    public ReservasController(IReservaFacade reservaFacade, ICommandInvoker commandInvoker, ICommandFactory commandFactory)
    {
        _reservaFacade = reservaFacade;
        _commandInvoker = commandInvoker;
        _commandFactory = commandFactory;
    }

    public IActionResult Index(string? status, string? tipoQuarto, string? hospede, DateTime? dataInicio, DateTime? dataFim)
    {
        var filtro = new FiltroReservas
        {
            Status = status,
            TipoQuarto = tipoQuarto,
            NomeHospede = hospede,
            DataInicio = dataInicio,
            DataFim = dataFim
        };

        // Iterator: percorre a coleção aplicando o filtro
        var reservas = _reservaFacade.ObterReservasFiltradas(filtro);

        ViewBag.Filtro = filtro;
        ViewBag.TiposQuarto = _reservaFacade.ObterPrototiposQuartos();
        ViewBag.StatusOpcoes = new[] { "Confirmada", "Check-in", "Check-out", "Cancelada" };
        return View(reservas);
    }

    public IActionResult Criar()
    {
        PrepararViewCriar();
        return View();
    }

    [HttpPost]
    public IActionResult Criar(string hospedeNome, string tipoQuarto, string tipoPacote,
                               DateTime dataEntrada, DateTime dataSaida, string metodoPagamento = "pix",
                               string? numeroCartao = null, string? cvv = null)
    {
        var resultado = _reservaFacade.CriarReservaComPacote(hospedeNome, tipoQuarto, tipoPacote, dataEntrada, dataSaida,
            metodoPagamento, numeroCartao, cvv);

        if (!resultado.Sucesso || resultado.Reserva == null)
        {
            PrepararViewCriar(hospedeNome, tipoQuarto, tipoPacote, dataEntrada, dataSaida,
                metodoPagamento, numeroCartao, cvv, null, resultado.MensagemErro);
            return View(nameof(Criar));
        }

        var pacoteMsg = string.IsNullOrEmpty(tipoPacote) ? "" : $" com pacote {tipoPacote}";
        TempData["Sucesso"] = $"Reserva #{resultado.Reserva.Id} confirmada{pacoteMsg}. Pagamento via {resultado.Reserva.MetodoPagamento} aprovado.";
        return RedirectToAction(nameof(Detalhes), new { id = resultado.Reserva.Id });
    }

    [HttpPost]
    public IActionResult CriarComDecorators(string hospedeNome, string tipoQuarto, string tipoPacote,
                                             DateTime dataEntrada, DateTime dataSaida,
                                             List<string> decorators, string metodoPagamento = "pix",
                                             string? numeroCartao = null, string? cvv = null)
    {
        decorators ??= new List<string>();

        var resultado = _reservaFacade.CriarReservaComPacoteEDecorators(
            hospedeNome, tipoQuarto, tipoPacote, dataEntrada, dataSaida, decorators,
            metodoPagamento, numeroCartao, cvv);

        if (!resultado.Sucesso || resultado.Reserva == null)
        {
            PrepararViewCriar(hospedeNome, tipoQuarto, tipoPacote, dataEntrada, dataSaida,
                metodoPagamento, numeroCartao, cvv, decorators, resultado.MensagemErro);
            return View(nameof(Criar));
        }

        var pacoteMsg = string.IsNullOrEmpty(tipoPacote) ? "" : $" com pacote {tipoPacote}";
        var decoratorsMsg = decorators.Any() ? $" + serviços extras: {string.Join(", ", decorators)}" : "";
        TempData["Sucesso"] = $"Reserva #{resultado.Reserva.Id} confirmada{pacoteMsg}{decoratorsMsg}. Equipes do hotel foram acionadas.";

        return RedirectToAction(nameof(Detalhes), new { id = resultado.Reserva.Id });
    }

    private void PrepararViewCriar(
        string? hospedeNome = null,
        string? tipoQuarto = null,
        string? tipoPacote = null,
        DateTime? dataEntrada = null,
        DateTime? dataSaida = null,
        string? metodoPagamento = null,
        string? numeroCartao = null,
        string? cvv = null,
        List<string>? decorators = null,
        string? mensagemErro = null)
    {
        ViewBag.Quartos = _reservaFacade.ObterPrototiposQuartos();
        ViewBag.TiposPacote = _reservaFacade.ObterTiposPacote();
        ViewBag.HospedeNome = hospedeNome ?? "";
        ViewBag.TipoQuarto = tipoQuarto ?? "";
        ViewBag.TipoPacote = tipoPacote ?? "";
        ViewBag.DataEntrada = dataEntrada?.ToString("yyyy-MM-dd") ?? "";
        ViewBag.DataSaida = dataSaida?.ToString("yyyy-MM-dd") ?? "";
        ViewBag.MetodoPagamento = metodoPagamento ?? "pix";
        ViewBag.NumeroCartao = numeroCartao ?? "";
        ViewBag.Cvv = cvv ?? "";
        ViewBag.Decorators = decorators ?? new List<string>();
        ViewBag.MensagemErro = mensagemErro;
    }

    public IActionResult Detalhes(string id)
    {
        var reserva = _reservaFacade.ObterReservaPorId(id);
        if (reserva == null)
            return NotFound();

        return View(reserva);
    }

    [HttpPost]
    public IActionResult CheckIn(string id)
    {
        var comando = _commandFactory.CreateCheckInCommand(id);
        _commandInvoker.ExecuteCommand(comando);

        var reserva = _reservaFacade.ObterReservaPorId(id);
        if (reserva != null && reserva.Status == "Check-in")
            TempData["Sucesso"] = "Check-in realizado. E-mail, Limpeza e Recepção foram atualizados.";
        else
            TempData["Erro"] = "Não foi possível realizar o check-in";

        return RedirectToAction(nameof(Detalhes), new { id });
    }

    [HttpPost]
    public IActionResult CheckOut(string id)
    {
        var comando = _commandFactory.CreateCheckOutCommand(id);
        _commandInvoker.ExecuteCommand(comando);

        var reserva = _reservaFacade.ObterReservaPorId(id);
        if (reserva != null && reserva.Status == "Check-out")
            TempData["Sucesso"] = "Check-out realizado. E-mail, Limpeza e Recepção foram atualizados.";
        else
            TempData["Erro"] = "Não foi possível realizar o check-out";

        return RedirectToAction(nameof(Detalhes), new { id });
    }

    [HttpPost]
    public IActionResult Cancelar(string id)
    {
        var comando = _commandFactory.CreateCancelarReservaCommand(id);
        _commandInvoker.ExecuteCommand(comando);

        var reserva = _reservaFacade.ObterReservaPorId(id);
        if (reserva != null && reserva.Status == "Cancelada")
            TempData["Sucesso"] = "Reserva cancelada. As equipes do hotel foram notificadas.";
        else
            TempData["Erro"] = "Não foi possível cancelar a reserva. Só é permitido cancelar enquanto estiver Confirmada.";

        return RedirectToAction(nameof(Detalhes), new { id });
    }

    [HttpPost]
    public IActionResult DesfazerUltimaAcao()
    {
        _commandInvoker.UndoLastCommand();
        TempData["Sucesso"] = "Última ação desfeita com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
