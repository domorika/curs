using System.ComponentModel.DataAnnotations;

namespace практы_курсак.Models;

public class BookingViewModel
{
    [Required(ErrorMessage = "Выберите услугу")]
    public int ServiceId { get; set; }

    [Required(ErrorMessage = "Введите ваше имя")]
    [Display(Name = "Ваше имя")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите телефон")]
    [Display(Name = "Телефон")]
    [Phone(ErrorMessage = "Введите корректный телефон")]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Введите корректный email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Выберите дату")]
    [Display(Name = "Дата съёмки")]
    [DataType(DataType.Date)]
    public DateTime BookingDate { get; set; }

    [Required(ErrorMessage = "Выберите время")]
    [Display(Name = "Время съёмки")]
    public string BookingTime { get; set; } = string.Empty;

    [Display(Name = "Примечания")]
    public string? Notes { get; set; }
}