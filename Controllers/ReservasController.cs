using Microsoft.AspNetCore.Mvc;
using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;
using SistemaHotelaria.Services;

namespace SistemaHotelaria.Controllers;

public class ReservasController : Controller
{
    private readonly GerenciadorReservas _gerenciador;
    private readonly HotelService _hotelService;

    public ReservasController(GerenciadorReservas gerenciador, HotelService hotelService)
    {
        _gerenciador = gerenciador;
        _hotelService = hotelService;
    }

    public IActionResult Index()
    {
        var reservas = _gerenciador.ObterTodasReservas();
        return View(reservas);
    }

    public IActionResult Criar()
    {
        ViewBag.Quartos = _hotelService.ObterPrototiposQuartos();
        ViewBag.TiposPacote = new[] { "Romantico", "Negocios", "Basico", "FimDeSemana" };
        return View();
    }

    [HttpPost]
    public IActionResult Criar(string hospedeNome, string tipoQuarto, string tipoPacote, 
                               DateTime dataEntrada, DateTime dataSaida)
    {
        // Se pacote foi selecionado mas quarto não, usar quarto Standard como padrão
        string tipoQuartoFinal = tipoQuarto;
        if (!string.IsNullOrEmpty(tipoPacote) && string.IsNullOrEmpty(tipoQuarto))
        {
            tipoQuartoFinal = "Standard";
        }
        
        var quarto = _hotelService.ObterPrototipoPorTipo(tipoQuartoFinal);
        if (quarto == null)
        {
            TempData["Erro"] = "Tipo de quarto não encontrado";
            return RedirectToAction(nameof(Criar));
        }

        var builder = _hotelService.CriarBuilder(tipoPacote);
        var director = new HotelDirector(builder);

        switch (tipoPacote)
        {
            case "Romantico":
                director.ConstruirPacoteRomanticoCompleto(quarto);
                break;
            case "Negocios":
                director.ConstruirPacoteNegociosCompleto(quarto);
                break;
            case "Basico":
                director.ConstruirPacoteBasico(quarto);
                break;
            case "FimDeSemana":
                director.ConstruirPacoteFimDeSemana(quarto);
                break;
            default:
                // Sem pacote - criar pacote básico apenas com o quarto
                director.ConstruirPacoteBasico(quarto);
                break;
        }

        var reserva = _gerenciador.CriarReservaWeb(hospedeNome, tipoQuartoFinal, dataEntrada, dataSaida, director.ObterPacote());
        
        if (reserva == null)
        {
            TempData["Erro"] = "Não há quartos disponíveis para este período";
            return RedirectToAction(nameof(Criar));
        }

        var pacoteMsg = string.IsNullOrEmpty(tipoPacote) ? "" : $" com pacote {tipoPacote}";
        TempData["Sucesso"] = $"Reserva #{reserva.Id} criada com sucesso{pacoteMsg}!";
        return RedirectToAction(nameof(Detalhes), new { id = reserva.Id });
    }

    public IActionResult Detalhes(string id)
    {
        var reserva = _gerenciador.ObterReservaPorId(id);
        if (reserva == null)
            return NotFound();
        
        return View(reserva);
    }

    [HttpPost]
    public IActionResult CheckIn(string id)
    {
        var sucesso = _gerenciador.RealizarCheckIn(id);
        if (sucesso)
            TempData["Sucesso"] = "Check-in realizado com sucesso!";
        else
            TempData["Erro"] = "Não foi possível realizar o check-in";
        
        return RedirectToAction(nameof(Detalhes), new { id });
    }

    [HttpPost]
    public IActionResult CheckOut(string id)
    {
        var sucesso = _gerenciador.RealizarCheckOut(id);
        if (sucesso)
            TempData["Sucesso"] = "Check-out realizado com sucesso!";
        else
            TempData["Erro"] = "Não foi possível realizar o check-out";
        
        return RedirectToAction(nameof(Detalhes), new { id });
    }
}
