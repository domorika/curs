using Microsoft.AspNetCore.Mvc;

namespace PhotoStudio.Controllers
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