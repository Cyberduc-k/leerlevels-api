using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = "1", Email = "JohnDoe@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1", Password = "J0nh#001!", Role = UserRole.Student, LastLogin = DateTime.Now, ShareCode = "DTRY-WQER-PIGU-VNSA", IsActive = true }
        );
    }
}
