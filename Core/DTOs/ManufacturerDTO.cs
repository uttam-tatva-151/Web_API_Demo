using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class ManufacturerDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Manufacturer name is required.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
    public string Country { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
