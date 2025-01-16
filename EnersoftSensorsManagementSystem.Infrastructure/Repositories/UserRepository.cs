using EnersoftSensorsManagementSystem.Core.Entities;
using EnersoftSensorsManagementSystem.Core.Interfaces;
using EnersoftSensorsManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Implementation of IUserRepository for managing user data.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly MssqlSensorDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(MssqlSensorDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    /// <inheritdoc />
    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
