using System.ComponentModel.DataAnnotations;

namespace практы_курсак.Models;

public class Service
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int DurationMinutes { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    public ICollection<Booking>? Bookings { get; set; }
}