using Microsoft.AspNetCore.Mvc;
using PhotoStudio.Models;
using System.Collections.Generic;
using System.Linq;

namespace PhotoStudio.Controllers
{
    public class ServicesController : Controller
    {
        private List<Service> GetServices()
        {
            return new List<Service>
            {
                new() { Id = 1, Name = "Семейная фотосессия", Price = 5000, DurationMinutes = 60 },
                new() { Id = 2, Name = "Портретная съёмка", Price = 4000, DurationMinutes = 45 },
                new() { Id = 3, Name = "Ретушь фото", Price = 1500, DurationMinutes = 30 },
                new() { Id = 4, Name = "Фотокнига", Price = 3500, DurationMinutes = 0 },
                new() { Id = 5, Name = "Рекламная съёмка", Price = 6500, DurationMinutes = 90 },
                new() { Id = 6, Name = "Свадебная фотосессия", Price = 7000, DurationMinutes = 90 },
                new() { Id = 7, Name = "История любви", Price = 4500, DurationMinutes = 45 },
                new() { Id = 8, Name = "Предметная съёмка", Price = 5000, DurationMinutes = 60 }
            };
        }

        public IActionResult Index(string sortBy = "Name")
        {
            var services = GetServices();

            services = sortBy switch
            {
                "Price" => services.OrderBy(s => s.Price).ToList(),
                "Duration" => services.OrderBy(s => s.DurationMinutes).ToList(),
                _ => services.OrderBy(s => s.Name).ToList()
            };

            ViewBag.SortBy = sortBy;
            return View(services);
        }
    }
}