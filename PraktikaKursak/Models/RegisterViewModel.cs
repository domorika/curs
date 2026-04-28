using System.ComponentModel.DataAnnotations;

namespace практы_курсак.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        [StringLength(50, ErrorMessage = "Фамилия не более 50 символов")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        [StringLength(50, ErrorMessage = "Имя не более 50 символов")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Отчество")]
        [StringLength(50, ErrorMessage = "Отчество не более 50 символов")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите телефон")]
        [Display(Name = "Телефон")]
        [Phone(ErrorMessage = "Введите корректный телефон")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}