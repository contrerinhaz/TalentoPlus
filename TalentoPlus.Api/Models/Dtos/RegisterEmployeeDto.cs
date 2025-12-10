using System.ComponentModel.DataAnnotations;

namespace TalentoPlus.Api.Models.Dtos;

public class RegisterEmployeeDto
{
    [Required]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public decimal Salary { get; set; }

    public string? ProfessionalProfile { get; set; }

    [Required]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    public int DepartmentId { get; set; }
}
