using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace практы_курсак.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Выберите дату")]
    [Display(Name = "Дата съёмки")]
    [DataType(DataType.Date)]
    public DateTime BookingDate { get; set; }

    [Required(ErrorMessage = "Выберите время")]
    [Display(Name = "Время съёмки")]
    public TimeSpan BookingTime { get; set; }

    [Required]
    [Display(Name = "Статус")]
    public string Status { get; set; } = "Pending";

    [Display(Name = "Примечания")]
    [StringLength(500)]
    public string? Notes { get; set; }

    [Display(Name = "Дата создания")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public int ServiceId { get; set; }

    [Required]
    public int ClientId { get; set; }

    [ForeignKey("ServiceId")]
    public Service? Service { get; set; }

    [ForeignKey("ClientId")]
    public Client? Client { get; set; }
}