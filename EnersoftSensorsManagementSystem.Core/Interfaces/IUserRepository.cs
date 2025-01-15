using EnersoftSensorsManagementSystem.Core.Entities;
using System.Threading.Tasks;

namespace EnersoftSensorsManagementSystem.Core.Interfaces;

/// <summary>
/// Interface for user data operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Adds a new user to the database.
    /// </summary>
    /// <param name="user">The user to add.</param>
    Task AddUserAsync(User user);
}
