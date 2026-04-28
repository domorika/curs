using System.ComponentModel.DataAnnotations;

namespace практы_курсак.Models;

public class Client
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите ФИО")]
    [Display(Name = "ФИО")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите телефон")]
    [Display(Name = "Телефон")]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    // Используем DateTimeKind.Unspecified для timestamp without time zone
    private DateTime _registeredAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

    public DateTime RegisteredAt
    {
        get => _registeredAt;
        set => _registeredAt = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
    }

    // Навигационное свойство
    public ICollection<Booking>? Bookings { get; set; }
}