using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TalentoPlus.Web.Entities;
using TalentoPlus.Web.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TalentoPlus.Web.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeesController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public EmployeesController(IEmployeeService employeeService, IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(int? pageNumber)
    {
        int pageSize = 10;
        var employees = await _employeeService.GetPaginatedEmployeesAsync(pageNumber ?? 1, pageSize);
        return View(employees);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = new SelectList(await _departmentService.GetAllDepartmentsAsync(), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeService.CreateEmployeeAsync(employee);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Departments = new SelectList(await _departmentService.GetAllDepartmentsAsync(), "Id", "Name", employee.DepartmentId);
        return View(employee);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
        if (employee == null)
        {
            return NotFound();
        }
        ViewBag.Departments = new SelectList(await _departmentService.GetAllDepartmentsAsync(), "Id", "Name", employee.DepartmentId);
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _employeeService.UpdateEmployeeAsync(employee);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Departments = new SelectList(await _departmentService.GetAllDepartmentsAsync(), "Id", "Name", employee.DepartmentId);
        return View(employee);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    public async Task<IActionResult> Resume(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    public async Task<IActionResult> DownloadResumePdf(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
        if (employee == null)
        {
            return NotFound();
        }

        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(QuestPDF.Helpers.PageSizes.A4);
                page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                page.PageColor(QuestPDF.Helpers.Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .AlignCenter()
                    .Column(col =>
                    {
                        col.Item().Text($"{employee.FirstName} {employee.LastName}").FontSize(24).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                        col.Item().Text(employee.JobTitle).FontSize(14);
                        col.Item().Text(employee.Department?.Name ?? "").FontSize(12).FontColor(QuestPDF.Helpers.Colors.Grey.Medium);
                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.Spacing(20);
                            row.AutoItem().Text(employee.Email);
                            row.AutoItem().Text(employee.PhoneNumber);
                            row.AutoItem().Text(employee.Address);
                        });
                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(QuestPDF.Helpers.Colors.Grey.Lighten2);
                    });

                page.Content()
                    .PaddingVertical(10)
                    .Row(row =>
                    {
                        // Left Column
                        row.RelativeItem(1).Column(col =>
                        {
                            col.Item().Text("Datos Personales").FontSize(14).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(QuestPDF.Helpers.Colors.Blue.Medium);

                            col.Item().Text("Documento:").Bold();
                            col.Item().Text(employee.DocumentNumber);
                            col.Item().PaddingBottom(5);

                            col.Item().Text("Fecha Nacimiento:").Bold();
                            col.Item().Text(employee.BirthDate.ToShortDateString());
                            col.Item().PaddingBottom(5);

                            col.Item().Text("Estado:").Bold();
                            col.Item().Text(employee.Status.ToString());
                            col.Item().PaddingBottom(15);

                            col.Item().Text("Educación").FontSize(14).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            col.Item().Text(employee.EducationLevel.ToString());
                        });

                        row.Spacing(20);

                        // Right Column
                        row.RelativeItem(2).Column(col =>
                        {
                            col.Item().Text("Perfil Profesional").FontSize(14).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            col.Item().Text(employee.ProfessionalProfile ?? "").Justify();
                            col.Item().PaddingBottom(15);

                            col.Item().Text("Información Laboral").FontSize(14).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(QuestPDF.Helpers.Colors.Blue.Medium);

                            col.Item().Text(employee.JobTitle ?? "").Bold().FontSize(12);
                            col.Item().Text(employee.Department?.Name ?? "").FontColor(QuestPDF.Helpers.Colors.Grey.Medium);
                            col.Item().Text($"Fecha de Ingreso: {employee.HireDate.ToShortDateString()}");
                            col.Item().Text($"Salario: {employee.Salary:C}");
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generado por TalentoPlus - ");
                        x.CurrentPageNumber();
                    });
            });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;
        return File(stream, "application/pdf", $"HojaVida_{employee.FirstName}_{employee.LastName}.pdf");
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _employeeService.DeleteEmployeeAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
