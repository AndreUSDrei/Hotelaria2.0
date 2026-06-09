using Microsoft.AspNetCore.Mvc;
using SistemaHotelaria.Services.Facade;

namespace SistemaHotelaria.Controllers;

public class ReservasController : Controller
{
    private readonly IReservaFacade _reservaFacade;

    public ReservasController(IReservaFacade reservaFacade)
    {
        _reservaFacade = reservaFacade;
    }

    public IActionResult Index()
    {
        var reservas = _reservaFacade.ObterTodasReservas();
        return View(reservas);
    }

    public IActionResult Criar()
    {
        ViewBag.Quartos = _reservaFacade.ObterPrototiposQuartos();
        ViewBag.TiposPacote = _reservaFacade.ObterTiposPacote();
        return View();
    }

    [HttpPost]
    public IActionResult Criar(string hospedeNome, string tipoQuarto, string tipoPacote,
                               DateTime dataEntrada, DateTime dataSaida)
    {
        var reserva = _reservaFacade.CriarReservaComPacote(hospedeNome, tipoQuarto, tipoPacote, dataEntrada, dataSaida);

        if (reserva == null)
        {
            var quartoInformado = !string.IsNullOrEmpty(tipoQuarto) || !string.IsNullOrEmpty(tipoPacote);
            TempData["Erro"] = quartoInformado
                ? "Não há quartos disponíveis para este período ou tipo de quarto inválido"
                : "Tipo de quarto não encontrado";
            return RedirectToAction(nameof(Criar));
        }

        var pacoteMsg = string.IsNullOrEmpty(tipoPacote) ? "" : $" com pacote {tipoPacote}";
        TempData["Sucesso"] = $"Reserva #{reserva.Id} criada com sucesso{pacoteMsg}!";
        return RedirectToAction(nameof(Detalhes), new { id = reserva.Id });
    }

    // ============================================================
    // NOVA ACTION - Criação de Reserva com Decorators
    // ============================================================
    // Esta action permite criar uma reserva com serviços extras
    // adicionados via Padrão Decorator.
    //
    // IMPORTANTE - PARÂMETRO decorators:
    // Recebe uma lista de strings do formulário (checkboxes marcados).
    // Cada string representa um decorator a ser aplicado.
    //
    // Isso demonstra como o padrão Decorator se integra com MVC:
    // - View: checkboxes para selecionar serviços extras
    // - Controller: recebe seleção e chama Facade com decorators
    // - Facade: aplica decorators usando o padrão
    // - Model: reserva é criada com serviços extras
    // ============================================================
    [HttpPost]
    public IActionResult CriarComDecorators(string hospedeNome, string tipoQuarto, string tipoPacote,
                                             DateTime dataEntrada, DateTime dataSaida,
                                             List<string> decorators)
    {
        // Se não houver decorators selecionados, inicializa lista vazia
        decorators ??= new List<string>();

        // Chama o método da Facade que integra Builder + Decorator
        var reserva = _reservaFacade.CriarReservaComPacoteEDecorators(
            hospedeNome, tipoQuarto, tipoPacote, dataEntrada, dataSaida, decorators);

        if (reserva == null)
        {
            var quartoInformado = !string.IsNullOrEmpty(tipoQuarto) || !string.IsNullOrEmpty(tipoPacote);
            TempData["Erro"] = quartoInformado
                ? "Não há quartos disponíveis para este período ou tipo de quarto inválido"
                : "Tipo de quarto não encontrado";
            return RedirectToAction(nameof(Criar));
        }

        // Monta mensagem de sucesso incluindo os decorators selecionados
        var pacoteMsg = string.IsNullOrEmpty(tipoPacote) ? "" : $" com pacote {tipoPacote}";
        var decoratorsMsg = decorators.Any() ? $" + serviços extras: {string.Join(", ", decorators)}" : "";
        TempData["Sucesso"] = $"Reserva #{reserva.Id} criada com sucesso{pacoteMsg}{decoratorsMsg}!";
        
        return RedirectToAction(nameof(Detalhes), new { id = reserva.Id });
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
        var sucesso = _reservaFacade.RealizarCheckIn(id);
        if (sucesso)
            TempData["Sucesso"] = "Check-in realizado com sucesso!";
        else
            TempData["Erro"] = "Não foi possível realizar o check-in";

        return RedirectToAction(nameof(Detalhes), new { id });
    }

    [HttpPost]
    public IActionResult CheckOut(string id)
    {
        var sucesso = _reservaFacade.RealizarCheckOut(id);
        if (sucesso)
            TempData["Sucesso"] = "Check-out realizado com sucesso!";
        else
            TempData["Erro"] = "Não foi possível realizar o check-out";

        return RedirectToAction(nameof(Detalhes), new { id });
    }
}
