using Moq;
using TalentoPlus.Web.Entities;
using TalentoPlus.Web.Repositories.Interfaces;
using TalentoPlus.Web.Services.Implementations;
using Xunit;

namespace TalentoPlus.Tests.UnitTests;

public class DepartmentServiceTests
{
    [Fact]
    public async Task GetAllDepartmentsAsync_ReturnsAllDepartments()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var departments = new List<Department>
        {
            new Department { Id = 1, Name = "HR" },
            new Department { Id = 2, Name = "IT" }
        };
        mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(departments);
        var service = new DepartmentService(mockRepo.Object);

        // Act
        var result = await service.GetAllDepartmentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, d => d.Name == "HR");
        Assert.Contains(result, d => d.Name == "IT");
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ReturnsDepartment_WhenExists()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var department = new Department { Id = 1, Name = "HR" };
        mockRepo.Setup(repo => repo.GetByIdWithDetailsAsync(1)).ReturnsAsync(department);
        var service = new DepartmentService(mockRepo.Object);

        // Act
        var result = await service.GetDepartmentByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("HR", result.Name);
    }
}
