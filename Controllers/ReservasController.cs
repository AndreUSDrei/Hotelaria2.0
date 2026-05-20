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
