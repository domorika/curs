using Microsoft.AspNetCore.Mvc;

namespace практы_курсак.Controllers
{
    public class PortfolioController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Портфолио";
            return View();
        }
    }
}