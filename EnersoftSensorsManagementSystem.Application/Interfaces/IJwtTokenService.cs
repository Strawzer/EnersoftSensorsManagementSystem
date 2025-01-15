namespace EnersoftSensorsManagementSystem.Application.Interfaces;

/// <summary>
/// Interface for generating JWT tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for a given username.
    /// </summary>
    /// <param name="username">The username for which to generate the token.</param>
    /// <returns>A JWT token string.</returns>
    string GenerateToken(string username, string role);
}
