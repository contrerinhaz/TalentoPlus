using TalentoPlus.Web.Entities;

namespace TalentoPlus.Web.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> GetAllWithDetailsAsync();
    Task<Employee?> GetByIdWithDetailsAsync(int id);
    Task<(IEnumerable<Employee> Items, int TotalCount)> GetPaginatedAsync(int pageIndex, int pageSize);
}
