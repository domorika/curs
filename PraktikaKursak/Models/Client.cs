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

    public DateTime RegisteredAt { get; set; } = DateTime.Now;
    public ICollection<Booking>? Bookings { get; set; }
}