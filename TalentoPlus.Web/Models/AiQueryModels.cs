namespace TalentoPlus.Web.Models;

public class AiQueryRequest
{
    public string Query { get; set; } = string.Empty;
}

public class AiQueryResponse
{
    public bool Success { get; set; }
    public string Answer { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}
