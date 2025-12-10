using System.ComponentModel.DataAnnotations;

namespace TalentoPlus.Web.Entities;

public class Department
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Department Name is required.")]
    [StringLength(100, ErrorMessage = "The Department Name cannot exceed 100 characters.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "The Department Name must contain only letters.")]
    public string Name { get; set; } = string.Empty;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
