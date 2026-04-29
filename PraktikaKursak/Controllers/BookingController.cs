using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using практы_курсак.Data;
using практы_курсак.Models;

namespace практы_курсак.Controllers;

public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Список бронирований
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin") || (user != null && user.IsAdmin);

        IQueryable<Booking> bookings = _context.Bookings
            .Include(b => b.Service)
            .Include(b => b.User);

        if (!isAdmin && user != null)
        {
            bookings = bookings.Where(b => b.UserId == user.Id);
        }

        ViewBag.IsAdmin = isAdmin;
        var bookingsList = await bookings
            .OrderByDescending(b => b.BookingDate)
            .ThenBy(b => b.BookingTime)
            .ToListAsync();

        return View(bookingsList);
    }

    // Форма создания бронирования
    // GET: Booking/Create — отображение формы
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.Services = await _context.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();

        return View(new BookingViewModel());
    }

    // POST: Booking/Create — обработка формы
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
            return View(model);
        }

        try
        {
            var bookingTime = TimeSpan.Parse(model.BookingTime);

            // Проверка занятости времени
            var isBusy = await _context.Bookings.AnyAsync(b =>
                b.BookingDate.Date == model.BookingDate.Date &&
                b.BookingTime == bookingTime &&
                b.Status != "Cancelled");

            if (isBusy)
            {
                ViewBag.Services = await _context.Services
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync();

                ModelState.AddModelError("", "Это время уже занято. Выберите другое время.");
                return View(model);
            }

            var booking = new Booking
            {
                ServiceId = model.ServiceId,
                UserId = user.Id,
                BookingDate = model.BookingDate.Date,
                BookingTime = bookingTime,
                Notes = model.Notes,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Бронирование успешно создано! Мы свяжемся с вами для подтверждения.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            ModelState.AddModelError("", $"Ошибка при создании бронирования: {ex.Message}");
            return View(model);
        }
    }

    // Подтверждение бронирования (только для администратора)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Confirm(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = "Confirmed";
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Бронирование #{id} подтверждено";
        }
        else
        {
            TempData["Error"] = "Бронирование не найдено";
        }
        return RedirectToAction(nameof(Index));
    }

    // Отмена бронирования (администратор может отменить любое, пользователь - только своё)
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin") || (user != null && user.IsAdmin);

        var booking = await _context.Bookings.FindAsync(id);

        if (booking == null)
        {
            TempData["Error"] = "Бронирование не найдено";
            return RedirectToAction(nameof(Index));
        }

        if (isAdmin || booking.UserId == user?.Id)
        {
            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Бронирование #{id} отменено";
        }
        else
        {
            TempData["Error"] = "У вас нет прав для отмены этого бронирования";
        }

        return RedirectToAction(nameof(Index));
    }

    // Удаление бронирования (только для администратора)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Бронирование #{id} удалено";
        }
        else
        {
            TempData["Error"] = "Бронирование не найдено";
        }
        return RedirectToAction(nameof(Index));
    }

    // Завершение бронирования (только для администратора)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Complete(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = "Completed";
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Бронирование #{id} отмечено как выполненное";
        }
        else
        {
            TempData["Error"] = "Бронирование не найдено";
        }
        return RedirectToAction(nameof(Index));
    }
}