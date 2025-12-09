using System.Text;
using System.Text.Json;
using TalentoPlus.Web.Models;
using TalentoPlus.Web.Services.Interfaces;

namespace TalentoPlus.Web.Services.Implementations;

public class GeminiAiService : IAiService
{
    private readonly string _apiKey;
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<GeminiAiService> _logger;
    private readonly HttpClient _httpClient;

    public GeminiAiService(
        IConfiguration configuration,
        IDashboardService dashboardService,
        ILogger<GeminiAiService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _apiKey = configuration["GeminiAI:ApiKey"] ?? throw new ArgumentNullException("GeminiAI:ApiKey is not configured");
        _dashboardService = dashboardService;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<AiQueryResponse> ProcessQueryAsync(string query)
    {
        try
        {
            // COMBINED PROMPT: Validate and Analyze in one go to save API calls
            var analysisPrompt = $@"Actúa como un experto en Recursos Humanos de TalentoPlus. Analiza la siguiente pregunta del usuario.
Tu tarea es devolver un JSON (sin markdown) con 3 campos:
1. 'isValid': true si la pregunta es sobre empleados, departamentos, cargos o RRHH. false si es sobre otro tema.
2. 'type': El tipo de consulta. Valores posibles: 'CARGO', 'DEPTO', 'ESTADO', 'GENERAL', 'INVALIDO'.
3. 'value': El valor específico a buscar (nombre del cargo, departamento o estado). Si es GENERAL o INVALIDO, pon null.

Pregunta: {query}";

            var jsonResponse = await CallGeminiApiAsync(analysisPrompt);

            // Clean markdown if present (```json ... ```)
            jsonResponse = jsonResponse.Replace("```json", "").Replace("```", "").Trim();

            using var doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;

            bool isValid = root.GetProperty("isValid").GetBoolean();
            string type = root.GetProperty("type").GetString()?.ToUpper() ?? "INVALIDO";
            string? value = root.GetProperty("value").GetString();

            if (!isValid)
            {
                return new AiQueryResponse
                {
                    Success = true,
                    Answer = "Lo siento, solo puedo responder preguntas relacionadas con empleados, departamentos y recursos humanos de TalentoPlus."
                };
            }

            _logger.LogInformation($"AI Analysis: Type={type}, Value={value}");

            // Process based on analysis WITHOUT making extra API calls for simple formatting
            if (type == "CARGO" && !string.IsNullOrEmpty(value))
            {
                var count = await _dashboardService.GetEmployeeCountByJobTitleAsync(value);
                return new AiQueryResponse
                {
                    Success = true,
                    Answer = $"Actualmente hay {count} empleado(s) con el cargo de {value} en la empresa."
                };
            }
            else if (type == "DEPTO" && !string.IsNullOrEmpty(value))
            {
                var count = await _dashboardService.GetEmployeeCountByDepartmentAsync(value);
                return new AiQueryResponse
                {
                    Success = true,
                    Answer = $"El departamento de {value} cuenta con {count} empleado(s) registrados."
                };
            }
            else if (type == "ESTADO" && !string.IsNullOrEmpty(value))
            {
                var count = await _dashboardService.GetEmployeeCountByStatusAsync(value);
                return new AiQueryResponse
                {
                    Success = true,
                    Answer = $"En este momento hay {count} empleado(s) en estado '{value}'."
                };
            }
            else
            {
                // Only for GENERAL questions we make a second call
                var generalPrompt = $@"Responde brevemente esta pregunta sobre empleados de TalentoPlus de forma útil y profesional.
Si no tienes información específica, sugiere al usuario usar el dashboard.
Pregunta: {query}";

                var generalResponse = await CallGeminiApiAsync(generalPrompt);
                return new AiQueryResponse
                {
                    Success = true,
                    Answer = generalResponse
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing AI query");
            return new AiQueryResponse
            {
                Success = false,
                ErrorMessage = "Error al procesar la consulta. Por favor, intenta de nuevo."
            };
        }
    }

    private async Task<string> CallGeminiApiAsync(string prompt)
    {
        // Using gemini-2.0-flash-exp which often has better availability/limits for free tier
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key={_apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Retry logic for 429 Too Many Requests
        int maxRetries = 3;
        int delay = 2000; // start with 2 seconds

        for (int i = 0; i < maxRetries; i++)
        {
            var response = await _httpClient.PostAsync(url, content);

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                _logger.LogWarning($"Rate limit hit (429). Retrying in {delay}ms...");
                await Task.Delay(delay);
                delay *= 2; // Exponential backoff
                continue;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Gemini API Error: {responseContent}");
                throw new Exception($"Error calling Gemini API: {response.StatusCode}");
            }

            var jsonResponse = JsonDocument.Parse(responseContent);

            // Handle cases where the model returns no content (safety filters, etc.)
            try
            {
                var text = jsonResponse.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text?.Trim() ?? "";
            }
            catch (Exception)
            {
                // Fallback if structure is different or empty
                return "No pude generar una respuesta válida.";
            }
        }

        throw new Exception("Gemini API is currently overloaded. Please try again later.");
    }
}
