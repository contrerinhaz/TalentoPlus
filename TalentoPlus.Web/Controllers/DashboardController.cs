using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Web.Models;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IAiService _aiService;

    public DashboardController(IDashboardService dashboardService, IAiService aiService)
    {
        _dashboardService = dashboardService;
        _aiService = aiService;
    }

    public async Task<IActionResult> Index()
    {
        var dashboardData = await _dashboardService.GetDashboardDataAsync();
        return View(dashboardData);
    }

    [HttpPost]
    public async Task<IActionResult> AskAi([FromBody] AiQueryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return BadRequest(new { error = "La pregunta no puede estar vacía" });
        }

        var response = await _aiService.ProcessQueryAsync(request.Query);
        return Json(response);
    }
}
