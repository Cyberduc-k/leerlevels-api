using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public class TargetContext : DbContext
{
    public DbSet<Target> Targets { get; set; }
    public DbSet<Mcq> Mcqs { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }

    public TargetContext(DbContextOptions contextOptions) : base(contextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnswerOption>().HasData(
            new { Id = "1", McqId = "1", Index = 0, IsCorrect = false, Text = "Beide voorwerpen zijn negatief geladen" },
            new { Id = "2", McqId = "1", Index = 1, IsCorrect = false, Text = "Beide voorwerpen zijn positief geladen" },
            new { Id = "3", McqId = "1", Index = 2, IsCorrect = false, Text = "Beide voorwerpen zijn neutraal geladen" },
            new { Id = "4", McqId = "1", Index = 3, IsCorrect = true, Text = "De lading is tegengesteld" }
        );

        modelBuilder.Entity<Mcq>().HasData(
            new { Id = "1", TargetId = "1", QuestionText = "", Explanation = "", AllowRandom = true }
        );

        modelBuilder.Entity<Target>().HasData(
            new { Id = "1", Label = "", Description = "", TargetExplanation = "", YoutubeId = "", ImageUrl = "" }
        );
    }
}
