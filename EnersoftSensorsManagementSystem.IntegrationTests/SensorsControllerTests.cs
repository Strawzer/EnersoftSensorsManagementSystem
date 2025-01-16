using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace EnersoftSensorsManagementSystem.IntegrationTests;

public class SensorsControllerTests : IClassFixture<WebApplicationFactory<API.Program>>
{
    private readonly HttpClient _client;
    private string _adminToken;

    public SensorsControllerTests(WebApplicationFactory<API.Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Login as Admin and retrieve the token
    private async Task LoginAsAdminAsync()
    {
        var loginRequest = new
        {
            username = "admin",
            password = "password" 
        };

        var response = await _client.PostAsync(
            "/api/v1.0/auth/login",
            new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json")
        );

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var tokenObject = JsonConvert.DeserializeAnonymousType(content, new { token = "", role = "" });

        _adminToken = tokenObject.token;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
    }

    [Fact]
    public async Task AddAndDeleteSensor_ShouldWorkCorrectly()
    {
        // Login as admin
        await LoginAsAdminAsync();

        // Add a sensor
        var sensor = new
        {
            name = "Test Sensor",
            location = "Test Location",
            isActive = true,
            typeId = 1 // Replace with a valid TypeId from your database
        };

        var addResponse = await _client.PostAsync(
            "/api/v1.0/sensors",
            new StringContent(JsonConvert.SerializeObject(sensor), Encoding.UTF8, "application/json")
        );

        addResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var addContent = await addResponse.Content.ReadAsStringAsync();
        var addedSensor = JsonConvert.DeserializeObject<dynamic>(addContent);
        int sensorId = (int)addedSensor.id;

        // Delete the sensor
        var deleteResponse = await _client.DeleteAsync($"/api/v1.0/sensors/{sensorId}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the sensor no longer exists
        var getResponse = await _client.GetAsync($"/api/v1.0/sensors/{sensorId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
