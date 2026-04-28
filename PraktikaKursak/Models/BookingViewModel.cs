using System.ComponentModel.DataAnnotations;

namespace практы_курсак.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Выберите услугу")]
        public int ServiceId { get; set; }

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
}