using TalentoPlus.Web.Models;

namespace TalentoPlus.Web.Services.Interfaces;

public interface IAiService
{
    Task<AiQueryResponse> ProcessQueryAsync(string query);
}

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardDataAsync();
    Task<int> GetEmployeeCountByJobTitleAsync(string jobTitle);
    Task<int> GetEmployeeCountByDepartmentAsync(string departmentName);
    Task<int> GetEmployeeCountByStatusAsync(string status);
}
