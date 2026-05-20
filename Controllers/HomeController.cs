using Microsoft.AspNetCore.Mvc;
using SistemaHotelaria.Services.Facade;

namespace SistemaHotelaria.Controllers;

public class HomeController : Controller
{
    private readonly IReservaFacade _reservaFacade;

    public HomeController(IReservaFacade reservaFacade)
    {
        _reservaFacade = reservaFacade;
    }

    public IActionResult Index()
    {
        var quartos = _reservaFacade.ObterPrototiposQuartos();
        ViewBag.ReservasAtivas = _reservaFacade.ContarReservasAtivas();
        return View(quartos);
    }

    public IActionResult Disponibilidade()
    {
        return View();
    }

    [HttpPost]
    public IActionResult VerificarDisponibilidade(DateTime entrada, DateTime saida)
    {
        var disponibilidade = _reservaFacade.ObterDisponibilidadeCompleta(entrada, saida);
        ViewBag.Entrada = entrada;
        ViewBag.Saida = saida;
        return View(disponibilidade);
    }

    public IActionResult Pacotes()
    {
        var quartos = _reservaFacade.ObterPrototiposQuartos();
        return View(quartos);
    }
}
