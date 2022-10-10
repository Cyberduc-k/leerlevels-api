using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public class ForumContext : DbContext
{
    public DbSet<Forum> Forums { get; set; }
    public DbSet<ForumReply> Replies { get; set; }

    public ForumContext(DbContextOptions<ForumContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
