using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Api.Models.Dtos;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        var dtos = departments.Select(d => new DepartmentDto { Id = d.Id, Name = d.Name });
        return Ok(dtos);
    }
}
