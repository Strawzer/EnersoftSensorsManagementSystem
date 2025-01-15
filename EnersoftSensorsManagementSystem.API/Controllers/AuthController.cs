using EnersoftSensorsManagementSystem.Application.DTOs;
using EnersoftSensorsManagementSystem.Application.Interfaces;
using EnersoftSensorsManagementSystem.Core.Entities;
using EnersoftSensorsManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnersoftSensorsManagementSystem.API.Controllers;

/// <summary>
/// API controller for authentication.
/// </summary>
[ApiController]
[Route(("api/v{version:apiVersion}/[controller]"))]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _tokenService;

    public AuthController(IUserRepository userRepository, IJwtTokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
        if (user == null) return Unauthorized();

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginRequest.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        // Pass the user's role to generate the token
        var token = _tokenService.GenerateToken(user.Username, user.Role);
        return Ok(new { Token = token, Role = user.Role });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequestDto loginRequest)
    {
        var passwordHasher = new PasswordHasher<User>();
        var user = new User
        {
            Username = loginRequest.Username,
            PasswordHash = passwordHasher.HashPassword(null, loginRequest.Password),
            Role = "User" // Default role
        };

        await _userRepository.AddUserAsync(user);
        return Ok(new { Message = "User registered successfully." });
    }
}
