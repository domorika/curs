using Microsoft.AspNetCore.Identity;

namespace практы_курсак.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Раздельное хранение ФИО
        public string LastName { get; set; } = string.Empty;   // Фамилия
        public string FirstName { get; set; } = string.Empty;  // Имя
        public string? MiddleName { get; set; }                // Отчество (необязательно)

        // Полное ФИО (вычисляемое свойство - не сохраняется в БД)
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(MiddleName))
                    return $"{LastName} {FirstName}";
                return $"{LastName} {FirstName} {MiddleName}".Trim();
            }
        }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public bool IsAdmin { get; set; } = false;

        // Навигационное свойство
        public ICollection<Booking>? Bookings { get; set; }
    }
}