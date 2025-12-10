using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalentoPlus.Web.Entities;
using TalentoPlus.Api.Models.Dtos;
using TalentoPlus.Web.Services.Interfaces;
using TalentoPlus.Api.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TalentoPlus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IEmailService _emailService;

    public EmployeesController(IEmployeeService employeeService, IEmailService emailService)
    {
        _employeeService = employeeService;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterEmployeeDto dto)
    {
        var employee = new Employee
        {
            DocumentNumber = dto.DocumentNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            HireDate = DateTime.SpecifyKind(dto.HireDate, DateTimeKind.Utc),
            BirthDate = DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc),
            Address = dto.Address,
            Salary = dto.Salary,
            ProfessionalProfile = dto.ProfessionalProfile,
            JobTitle = dto.JobTitle,
            DepartmentId = dto.DepartmentId,
            Status = EmployeeStatus.Active,
            EducationLevel = EducationLevel.Professional // Default
        };

        try
        {
            await _employeeService.CreateEmployeeAsync(employee);

            var subject = "Bienvenido a TalentoPlus";
            var body = TalentoPlus.Api.Helpers.EmailTemplates.GetWelcomeEmail(dto.FirstName);

            await _emailService.SendEmailAsync(dto.Email, subject, body);

            return Ok(new { Message = "Registro exitoso. Se ha enviado un correo de bienvenida." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Error al registrar empleado: " + ex.Message });
        }
    }

    [HttpPost("send-welcome/{id}")]
    public async Task<IActionResult> SendWelcomeEmail(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound("Empleado no encontrado.");
        }

        try
        {
            var subject = "Bienvenido a TalentoPlus";
            var body = TalentoPlus.Api.Helpers.EmailTemplates.GetWelcomeEmail(employee.FirstName);

            await _emailService.SendEmailAsync(employee.Email, subject, body);

            return Ok(new { Message = $"Correo de bienvenida enviado a {employee.Email}" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Error al enviar correo: " + ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyInfo()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized();
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(userId);
        if (employee == null)
        {
            return NotFound("Empleado no encontrado.");
        }

        return Ok(employee);
    }

    [Authorize]
    [HttpGet("me/resume")]
    public async Task<IActionResult> DownloadResume()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized();
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(userId);
        if (employee == null)
        {
            return NotFound("Empleado no encontrado.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .AlignCenter()
                    .Column(col =>
                    {
                        col.Item().Text($"{employee.FirstName} {employee.LastName}").FontSize(24).Bold().FontColor(Colors.Blue.Medium);
                        col.Item().Text(employee.JobTitle).FontSize(14);
                        col.Item().Text(employee.Department?.Name ?? "").FontSize(12).FontColor(Colors.Grey.Medium);
                        col.Item().PaddingTop(10).Row(row =>
                        {
                            row.Spacing(20);
                            row.AutoItem().Text(employee.Email);
                            row.AutoItem().Text(employee.PhoneNumber);
                            row.AutoItem().Text(employee.Address);
                        });
                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                page.Content()
                    .PaddingVertical(10)
                    .Row(row =>
                    {
                        // Left Column
                        row.RelativeItem(1).Column(col =>
                        {
                            col.Item().Text("Datos Personales").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                            col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);

                            col.Item().Text("Documento:").Bold();
                            col.Item().Text(employee.DocumentNumber);
                            col.Item().PaddingBottom(5);

                            col.Item().Text("Fecha Nacimiento:").Bold();
                            col.Item().Text(employee.BirthDate.ToShortDateString());
                            col.Item().PaddingBottom(5);

                            col.Item().Text("Estado:").Bold();
                            col.Item().Text(employee.Status.ToString());
                            col.Item().PaddingBottom(15);

                            col.Item().Text("Educación").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                            col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);
                            col.Item().Text(employee.EducationLevel.ToString());
                        });

                        row.Spacing(20);

                        // Right Column
                        row.RelativeItem(2).Column(col =>
                        {
                            col.Item().Text("Perfil Profesional").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                            col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);
                            col.Item().Text(employee.ProfessionalProfile ?? "").Justify();
                            col.Item().PaddingBottom(15);

                            col.Item().Text("Información Laboral").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                            col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);

                            col.Item().Text(employee.JobTitle ?? "").Bold().FontSize(12);
                            col.Item().Text(employee.Department?.Name ?? "").FontColor(Colors.Grey.Medium);
                            col.Item().Text($"Fecha de Ingreso: {employee.HireDate.ToShortDateString()}");
                            col.Item().Text($"Salario: {employee.Salary:C}");
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generado por TalentoPlus API - ");
                        x.CurrentPageNumber();
                    });
            });
        });

        var pdfBytes = document.GeneratePdf();
        return File(pdfBytes, "application/pdf", $"CV_{employee.FirstName}_{employee.LastName}.pdf");
    }
}
