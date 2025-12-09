using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Web.Entities;
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
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartment(int id)
    {
        var department = await _departmentService.GetDepartmentByIdAsync(id);

        if (department == null)
        {
            return NotFound();
        }

        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<Department>> PostDepartment(Department department)
    {
        await _departmentService.CreateDepartmentAsync(department);
        return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDepartment(int id, Department department)
    {
        if (id != department.Id)
        {
            return BadRequest();
        }

        await _departmentService.UpdateDepartmentAsync(department);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        await _departmentService.DeleteDepartmentAsync(id);
        return NoContent();
    }
}
