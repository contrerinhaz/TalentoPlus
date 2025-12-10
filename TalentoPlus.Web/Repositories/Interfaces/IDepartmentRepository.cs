using TalentoPlus.Web.Entities;

namespace TalentoPlus.Web.Repositories.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<IEnumerable<Department>> GetAllWithDetailsAsync();
    Task<Department?> GetByIdWithDetailsAsync(int id);
    Task<(IEnumerable<Department> Items, int TotalCount)> GetPaginatedAsync(int pageIndex, int pageSize);
}
