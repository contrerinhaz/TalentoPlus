using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Data;
using TalentoPlus.Web.Entities;
using TalentoPlus.Web.Repositories.Interfaces;

namespace TalentoPlus.Web.Repositories.Implementations;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Department>> GetAllWithDetailsAsync()
    {
        return await _dbSet.Include(d => d.Employees).ToListAsync();
    }

    public async Task<Department?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet.Include(d => d.Employees)
                           .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<(IEnumerable<Department> Items, int TotalCount)> GetPaginatedAsync(int pageIndex, int pageSize)
    {
        var query = _dbSet.AsQueryable();
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();
        return (items, totalCount);
    }
}
