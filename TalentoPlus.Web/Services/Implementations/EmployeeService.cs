using TalentoPlus.Web.Entities;
using TalentoPlus.Web.Repositories.Interfaces;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _employeeRepository.GetAllWithDetailsAsync();
    }

    public async Task<Models.PaginatedList<Employee>> GetPaginatedEmployeesAsync(int pageIndex, int pageSize)
    {
        var (items, count) = await _employeeRepository.GetPaginatedAsync(pageIndex, pageSize);
        return new Models.PaginatedList<Employee>(items.ToList(), count, pageIndex, pageSize);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _employeeRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task CreateEmployeeAsync(Employee employee)
    {
        await _employeeRepository.AddAsync(employee);
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        var existingEmployee = await _employeeRepository.GetByIdAsync(employee.Id);

        if (existingEmployee != null)
        {
            // Update fields only if they are provided (or update all if that's the expected behavior for a full edit form)
            // Since this is coming from a full edit form, we usually want to update everything that is valid.
            // But to support "PATCH-like" behavior where nulls don't overwrite existing data:

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.DocumentNumber = employee.DocumentNumber;
            existingEmployee.Email = employee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber;

            // Fix for PostgreSQL DateTime issue: Ensure dates are UTC
            existingEmployee.HireDate = DateTime.SpecifyKind(employee.HireDate, DateTimeKind.Utc);
            existingEmployee.BirthDate = DateTime.SpecifyKind(employee.BirthDate, DateTimeKind.Utc);

            existingEmployee.Address = employee.Address;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.JobTitle = employee.JobTitle;
            existingEmployee.Status = employee.Status;
            existingEmployee.EducationLevel = employee.EducationLevel;
            existingEmployee.DepartmentId = employee.DepartmentId;

            // Only update ProfessionalProfile if it's not null (optional field)
            if (employee.ProfessionalProfile != null)
            {
                existingEmployee.ProfessionalProfile = employee.ProfessionalProfile;
            }

            await _employeeRepository.UpdateAsync(existingEmployee);
        }
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        await _employeeRepository.DeleteAsync(id);
    }
}
