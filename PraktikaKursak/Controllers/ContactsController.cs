using Microsoft.AspNetCore.Mvc;

namespace практы_курсак.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Контакты";
            return View();
        }
    }
}