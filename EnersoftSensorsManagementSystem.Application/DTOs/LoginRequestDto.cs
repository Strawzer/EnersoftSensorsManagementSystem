namespace EnersoftSensorsManagementSystem.Application.DTOs;

/// <summary>
/// Data Transfer Object for login requests.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// The username for authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password for authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
