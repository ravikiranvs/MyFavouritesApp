namespace MyFavouritesRepository;

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFavouritesEntities;
using MyFavouritesEntities.Models;

public interface IUserRepository
{
    public Task<User?> GetUserByUserNameAsync(string userName);

    public Task<int> DeleteUserByIdAsync(int id);

    public Task<int> UpdateOrCreateUserByIdAsync(User user);
}

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly AWSMySQL dbContext;
    public UserRepository(ILogger<UserRepository> logger, AWSMySQL dbContext)
    {
        _logger = logger;
        this.dbContext = dbContext;
    }

    public async Task<int> DeleteUserByIdAsync(int id)
    {
        User user = new User() { Id = id };
        dbContext.Users.Attach(user);
        dbContext.Users.Remove(user);
        var result = await dbContext.SaveChangesAsync();

        return result;
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await dbContext.Users
            .Where(u => u.UserName == userName.ToLower())
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<int> UpdateOrCreateUserByIdAsync(User user)
    {
        if(user.Id > 0)
        {
            dbContext.Users.Update(user);
        }
        else
        {
            dbContext.Users.Add(user);
        }
        var result = await dbContext.SaveChangesAsync();

        return user.Id;
    }
}

