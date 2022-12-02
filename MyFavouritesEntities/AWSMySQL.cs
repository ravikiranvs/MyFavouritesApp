using Microsoft.EntityFrameworkCore;
using MyFavouritesEntities.Models;

namespace MyFavouritesEntities;
public class AWSMySQL : DbContext
{
    public AWSMySQL(DbContextOptions<AWSMySQL> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    //public DbSet<Token> Tokens { get; set; }
}
