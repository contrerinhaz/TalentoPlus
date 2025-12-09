using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Controllers;

[Authorize(Roles = "Admin")]
public class UploadController : Controller
{
    private readonly IExcelService _excelService;

    public UploadController(IExcelService excelService)
    {
        _excelService = excelService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("File", "Please select a valid Excel file.");
            return View("Index");
        }

        if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("File", "Only .xlsx files are allowed.");
            return View("Index");
        }

        try
        {
            using (var stream = file.OpenReadStream())
            {
                await _excelService.ProcessEmployeeFileAsync(stream);
            }
            TempData["SuccessMessage"] = "Employees imported successfully!";
            return RedirectToAction("Index", "Employees");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("File", $"An error occurred while processing the file: {ex.Message}");
            return View("Index");
        }
    }
}
