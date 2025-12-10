using OfficeOpenXml;
using TalentoPlus.Web.Entities;
using TalentoPlus.Web.Repositories.Interfaces;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations;

public class ExcelService : IExcelService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public ExcelService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task ProcessEmployeeFileAsync(Stream fileStream)
    {
        using var package = new ExcelPackage(fileStream);
        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;

        // Cache departments to avoid repeated DB calls
        var departments = (await _departmentRepository.GetAllAsync()).ToList();

        for (int row = 2; row <= rowCount; row++)
        {
            var documentNumber = worksheet.Cells[row, 1].Text;
            if (string.IsNullOrEmpty(documentNumber)) continue;

            var firstName = worksheet.Cells[row, 2].Text;
            var lastName = worksheet.Cells[row, 3].Text;
            var birthDateText = worksheet.Cells[row, 4].Text;
            var address = worksheet.Cells[row, 5].Text;
            var phone = worksheet.Cells[row, 6].Text;
            var email = worksheet.Cells[row, 7].Text;
            var jobTitle = worksheet.Cells[row, 8].Text;
            var salaryText = worksheet.Cells[row, 9].Text;
            var hireDateText = worksheet.Cells[row, 10].Text;
            var statusText = worksheet.Cells[row, 11].Text;
            var educationText = worksheet.Cells[row, 12].Text;
            var profile = worksheet.Cells[row, 13].Text;
            var departmentName = worksheet.Cells[row, 14].Text;

            // Find or create department
            var department = departments.FirstOrDefault(d => d.Name.Equals(departmentName, StringComparison.OrdinalIgnoreCase));
            if (department == null)
            {
                department = new Department { Name = departmentName };
                await _departmentRepository.AddAsync(department);
                departments.Add(department);
            }

            // Check if employee exists
            var existingEmployees = await _employeeRepository.FindAsync(e => e.DocumentNumber == documentNumber || e.Email == email);
            var employee = existingEmployees.FirstOrDefault();

            if (employee == null)
            {
                employee = new Employee();
            }

            // Map properties
            employee.DocumentNumber = documentNumber;
            employee.FirstName = firstName;
            employee.LastName = lastName;
            employee.Address = address;
            employee.PhoneNumber = phone;
            employee.Email = email;
            employee.JobTitle = jobTitle;
            employee.ProfessionalProfile = profile;
            employee.DepartmentId = department.Id;

            if (DateTime.TryParse(birthDateText, out DateTime birthDate))
                employee.BirthDate = birthDate.ToUniversalTime();

            if (DateTime.TryParse(hireDateText, out DateTime hireDate))
                employee.HireDate = hireDate.ToUniversalTime();

            if (decimal.TryParse(salaryText, out decimal salary))
                employee.Salary = salary;

            employee.Status = ParseEmployeeStatus(statusText);
            employee.EducationLevel = ParseEducationLevel(educationText);

            if (employee.Id == 0)
            {
                await _employeeRepository.AddAsync(employee);
            }
            else
            {
                await _employeeRepository.UpdateAsync(employee);
            }
        }
    }

    private EmployeeStatus ParseEmployeeStatus(string status)
    {
        return status.ToLower() switch
        {
            "activo" => EmployeeStatus.Active,
            "inactivo" => EmployeeStatus.Inactive,
            "vacaciones" => EmployeeStatus.OnLeave,
            _ => EmployeeStatus.Active
        };
    }

    private EducationLevel ParseEducationLevel(string education)
    {
        return education.ToLower() switch
        {
            "bachiller" => EducationLevel.HighSchool,
            "técnico" => EducationLevel.Technical,
            "tecnólogo" => EducationLevel.Technologist,
            "profesional" => EducationLevel.Professional,
            "especialización" => EducationLevel.Specialization,
            "maestría" => EducationLevel.Master,
            "doctorado" => EducationLevel.Doctorate,
            _ => EducationLevel.Professional
        };
    }
}
