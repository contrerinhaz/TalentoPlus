using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace TalentoPlus.Tests.IntegrationTests;

public class ApiEndpointsTests : IClassFixture<WebApplicationFactory<TalentoPlus.Api.Controllers.DepartmentsController>>
{
    private readonly WebApplicationFactory<TalentoPlus.Api.Controllers.DepartmentsController> _factory;

    public ApiEndpointsTests(WebApplicationFactory<TalentoPlus.Api.Controllers.DepartmentsController> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_Departments_ReturnsSuccessAndJsonContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/departments");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginDto = new { DocumentNumber = "000000", Email = "invalid@example.com" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(loginDto), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/auth/login", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
