using Microsoft.AspNetCore.Mvc;

namespace SistemaHotelaria.Controllers;

/// <summary>
/// Simplified Home Controller for pattern demonstration.
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Message = "Sistema de Hotelaria - Demonstração de Padrões de Projeto";
        ViewBag.Patterns = new[] { "Observer", "Strategy", "Composite" };
        return View();
    }
}
