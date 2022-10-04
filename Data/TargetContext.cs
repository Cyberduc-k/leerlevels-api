using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public class TargetContext : DbContext
{
    public DbSet<Target> Targets { get; set; }
    public DbSet<Mcq> Mcqs { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
}
