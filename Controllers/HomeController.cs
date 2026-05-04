using Microsoft.AspNetCore.Mvc;
using SistemaHotelaria.Services;

namespace SistemaHotelaria.Controllers;

public class HomeController : Controller
{
    private readonly HotelService _hotelService;
    private readonly GerenciadorReservas _gerenciador;

    public HomeController(HotelService hotelService, GerenciadorReservas gerenciador)
    {
        _hotelService = hotelService;
        _gerenciador = gerenciador;
    }

    public IActionResult Index()
    {
        var quartos = _hotelService.ObterPrototiposQuartos();
        ViewBag.ReservasAtivas = _gerenciador.ObterTodasReservas().Count;
        return View(quartos);
    }

    public IActionResult Disponibilidade()
    {
        return View();
    }

    [HttpPost]
    public IActionResult VerificarDisponibilidade(DateTime entrada, DateTime saida)
    {
        var disponibilidade = _gerenciador.ObterDisponibilidadeCompleta(entrada, saida);
        ViewBag.Entrada = entrada;
        ViewBag.Saida = saida;
        return View(disponibilidade);
    }

    public IActionResult Pacotes()
    {
        var quartos = _hotelService.ObterPrototiposQuartos();
        return View(quartos);
    }
}
