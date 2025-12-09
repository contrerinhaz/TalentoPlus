namespace TalentoPlus.Web.Models;

public class DashboardViewModel
{
    public int TotalEmployees { get; set; }
    public int EmployeesOnLeave { get; set; }
    public int ActiveEmployees { get; set; }
    public Dictionary<string, int> EmployeesByDepartment { get; set; } = new();
    public Dictionary<string, int> EmployeesByStatus { get; set; } = new();
    public List<TopSalaryEmployee> TopSalaries { get; set; } = new();
}

public class TopSalaryEmployee
{
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string Department { get; set; } = string.Empty;
}
