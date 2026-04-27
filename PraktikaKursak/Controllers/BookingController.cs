using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using практы_курсак.Data;
using практы_курсак.Models;

namespace практы_курсак.Controllers;

public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingController> _logger;

    public BookingController(ApplicationDbContext context, ILogger<BookingController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Список всех бронирований
    public async Task<IActionResult> Index()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Service)
            .Include(b => b.Client)
            .OrderByDescending(b => b.BookingDate)
            .ThenBy(b => b.BookingTime)
            .ToListAsync();

        return View(bookings);
    }

    // Форма создания бронирования
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Services = await _context.Services
            .Where(s => s.IsActive)
            .ToListAsync();

        return View(new BookingViewModel());
    }

    // Обработка создания бронирования
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingViewModel model)
    {
        // Убираем проверку ModelState для сложных полей
        if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Phone) || model.ServiceId == 0)
        {
            ViewBag.Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
            ModelState.AddModelError("", "Заполните все обязательные поля");
            return View(model);
        }

        try
        {
            // Преобразование времени
            TimeSpan bookingTime;
            try
            {
                bookingTime = TimeSpan.Parse(model.BookingTime);
            }
            catch
            {
                bookingTime = TimeSpan.FromHours(12); // значение по умолчанию
            }

            // Проверка на занятость времени
            var isBusy = await _context.Bookings.AnyAsync(b =>
                b.BookingDate.Date == model.BookingDate.Date &&
                b.BookingTime == bookingTime &&
                b.Status != "Cancelled");

            if (isBusy)
            {
                ViewBag.Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
                ModelState.AddModelError("", "Это время уже занято. Выберите другое время.");
                return View(model);
            }

            // Поиск существующего клиента
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Phone == model.Phone);

            // Если клиент не найден, создаём нового
            if (client == null)
            {
                client = new Client
                {
                    FullName = model.FullName.Trim(),
                    Phone = model.Phone.Trim(),
                    Email = model.Email?.Trim(),
                    RegisteredAt = DateTime.Now
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Обновляем данные существующего клиента если изменились
                if (client.FullName != model.FullName)
                    client.FullName = model.FullName;
                if (client.Email != model.Email)
                    client.Email = model.Email;

                _context.Clients.Update(client);
            }

            // Создание бронирования
            var booking = new Booking
            {
                ServiceId = model.ServiceId,
                ClientId = client.Id,
                BookingDate = model.BookingDate.Date,
                BookingTime = bookingTime,
                Notes = model.Notes,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Бронирование успешно создано! Мы свяжемся с вами для подтверждения.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании бронирования");
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            ModelState.AddModelError("", $"Ошибка: {errorMessage}");

            ViewBag.Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
            return View(model);
        }
    }

    // Подтверждение бронирования
    [HttpPost]
    public async Task<IActionResult> Confirm(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = "Confirmed";
            await _context.SaveChangesAsync();
            TempData["Success"] = "Бронирование подтверждено";
        }
        return RedirectToAction(nameof(Index));
    }

    // Отмена бронирования
    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
            TempData["Success"] = "Бронирование отменено";
        }
        return RedirectToAction(nameof(Index));
    }

    // Удаление бронирования
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Бронирование удалено";
        }
        return RedirectToAction(nameof(Index));
    }
}