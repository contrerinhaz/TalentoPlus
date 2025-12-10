using TalentoPlus.Web.Entities;
using TalentoPlus.Web.Repositories.Interfaces;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        return await _departmentRepository.GetAllAsync();
    }

    public async Task<Models.PaginatedList<Department>> GetPaginatedDepartmentsAsync(int pageIndex, int pageSize)
    {
        var (items, count) = await _departmentRepository.GetPaginatedAsync(pageIndex, pageSize);
        return new Models.PaginatedList<Department>(items.ToList(), count, pageIndex, pageSize);
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        return await _departmentRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task CreateDepartmentAsync(Department department)
    {
        await _departmentRepository.AddAsync(department);
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        await _departmentRepository.UpdateAsync(department);
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        await _departmentRepository.DeleteAsync(id);
    }
}
