using TalentoPlus.Web.Entities;

namespace TalentoPlus.Web.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    Task<Models.PaginatedList<Department>> GetPaginatedDepartmentsAsync(int pageIndex, int pageSize);
    Task<Department?> GetDepartmentByIdAsync(int id);
    Task CreateDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(int id);
}
