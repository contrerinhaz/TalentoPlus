using TalentoPlus.Web.Entities;

namespace TalentoPlus.Web.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Models.PaginatedList<Employee>> GetPaginatedEmployeesAsync(int pageIndex, int pageSize);
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task CreateEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
}
