using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace EnersoftSensorsManagementSystem.IntegrationTests;

public class SensorsControllerTests : IClassFixture<WebApplicationFactory<API.Program>>
{
    private readonly HttpClient _client;

    public SensorsControllerTests(WebApplicationFactory<API.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DeleteSensor_ShouldReturnNoContent_WhenSensorExists()
    {
        // Arrange
        var sensorId = 1;

        // Act
        var response = await _client.DeleteAsync($"/api/sensors/{sensorId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteSensor_ShouldReturnNotFound_WhenSensorDoesNotExist()
    {
        // Arrange
        var sensorId = 999; // Non-existent sensor

        // Act
        var response = await _client.DeleteAsync($"/api/sensors/{sensorId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}