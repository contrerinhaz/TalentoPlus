using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Data;
using TalentoPlus.Web.Entities;
using TalentoPlus.Web.Models;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var employees = await _context.Employees.Include(e => e.Department).ToListAsync();

        var viewModel = new DashboardViewModel
        {
            TotalEmployees = employees.Count,
            EmployeesOnLeave = employees.Count(e => e.Status == EmployeeStatus.OnLeave),
            ActiveEmployees = employees.Count(e => e.Status == EmployeeStatus.Active),
            EmployeesByDepartment = employees
                .GroupBy(e => e.Department?.Name ?? "Sin Departamento")
                .ToDictionary(g => g.Key, g => g.Count()),
            EmployeesByStatus = employees
                .GroupBy(e => e.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count()),
            TopSalaries = employees
                .OrderByDescending(e => e.Salary)
                .Take(5)
                .Select(e => new TopSalaryEmployee
                {
                    FullName = $"{e.FirstName} {e.LastName}",
                    JobTitle = e.JobTitle ?? "",
                    Salary = e.Salary,
                    Department = e.Department?.Name ?? "N/A"
                })
                .ToList()
        };

        return viewModel;
    }

    public async Task<int> GetEmployeeCountByJobTitleAsync(string jobTitle)
    {
        return await _context.Employees
            .Where(e => EF.Functions.ILike(e.JobTitle, $"%{jobTitle}%"))
            .CountAsync();
    }

    public async Task<int> GetEmployeeCountByDepartmentAsync(string departmentName)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Where(e => e.Department != null && EF.Functions.ILike(e.Department.Name, $"%{departmentName}%"))
            .CountAsync();
    }

    public async Task<int> GetEmployeeCountByStatusAsync(string status)
    {
        // Try to parse the status
        if (Enum.TryParse<EmployeeStatus>(status, true, out var employeeStatus))
        {
            return await _context.Employees.CountAsync(e => e.Status == employeeStatus);
        }

        // Handle Spanish translations
        var normalizedStatus = status.ToLower();
        if (normalizedStatus.Contains("activo") || normalizedStatus.Contains("active"))
        {
            return await _context.Employees.CountAsync(e => e.Status == EmployeeStatus.Active);
        }
        if (normalizedStatus.Contains("inactivo") || normalizedStatus.Contains("inactive"))
        {
            return await _context.Employees.CountAsync(e => e.Status == EmployeeStatus.Inactive);
        }
        if (normalizedStatus.Contains("licencia") || normalizedStatus.Contains("leave") || normalizedStatus.Contains("vacaciones"))
        {
            return await _context.Employees.CountAsync(e => e.Status == EmployeeStatus.OnLeave);
        }

        return 0;
    }
}
