using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentoPlus.Web.Entities;

public class Employee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Document Number is required.")]
    [StringLength(20, ErrorMessage = "The Document Number cannot exceed 20 characters.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "The Document Number must contain only numbers.")]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "The First Name is required.")]
    [StringLength(50, ErrorMessage = "The First Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "The First Name must contain only letters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Last Name is required.")]
    [StringLength(50, ErrorMessage = "The Last Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "The Last Name must contain only letters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid Phone Number.")]
    [RegularExpression(@"^[0-9\-\+\s]+$", ErrorMessage = "The Phone Number contains invalid characters.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "The Hire Date is required.")]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "The Birth Date is required.")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "The Address is required.")]
    [StringLength(200, ErrorMessage = "The Address cannot exceed 200 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Salary is required.")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "The Salary must be a positive number.")]
    public decimal Salary { get; set; }

    [StringLength(500, ErrorMessage = "The Professional Profile cannot exceed 500 characters.")]
    public string? ProfessionalProfile { get; set; }

    [Required(ErrorMessage = "The Job Title is required.")]
    [StringLength(100, ErrorMessage = "The Job Title cannot exceed 100 characters.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-\.]+$", ErrorMessage = "The Job Title contains invalid characters.")]
    public string JobTitle { get; set; } = string.Empty;

    public EmployeeStatus Status { get; set; }

    public EducationLevel EducationLevel { get; set; }

    [Required(ErrorMessage = "The Department is required.")]
    public int DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }
}
