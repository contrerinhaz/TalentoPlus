using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Data;
using TalentoPlus.Web.Entities;
using TalentoPlus.Web.Repositories.Interfaces;

namespace TalentoPlus.Web.Repositories.Implementations;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Employee>> GetAllWithDetailsAsync()
    {
        return await _dbSet.Include(e => e.Department).ToListAsync();
    }

    public async Task<Employee?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet.Include(e => e.Department)
                           .FirstOrDefaultAsync(e => e.Id == id);
    }
}
