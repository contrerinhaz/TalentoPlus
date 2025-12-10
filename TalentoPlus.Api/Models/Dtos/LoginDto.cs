using System.ComponentModel.DataAnnotations;

namespace TalentoPlus.Api.Models.Dtos;

public class LoginDto
{
    [Required]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
