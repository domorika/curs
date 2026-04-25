using Microsoft.AspNetCore.Mvc;

namespace PhotoStudio.Controllers
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