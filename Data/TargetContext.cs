using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public class TargetContext : DbContext
{
    public DbSet<Target> Targets { get; set; }
    public DbSet<Mcq> Mcqs { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }

    public TargetContext(DbContextOptions<TargetContext> contextOptions) : base(contextOptions)
    {
        Database.EnsureCreated();
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
            new { Id = "1", TargetId = "1", QuestionText = "Wat kun je zeggen over de lading van twee voorwerpen die elkaar aantrekken?", Explanation = "Voorwerpen met gelijksoortige lading (beide positief of beide negatief) stoten elkaar af. Voorwerpen met tegengestelde lading (een positief, een negatief) trekken elkaar aan.", AllowRandom = true }
        );

        modelBuilder.Entity<Target>().HasData(
            new { Id = "1", Label = "Lading concept", Description = "Je kan in eigen woorden uitleggen welk effect lading kan hebben.", TargetExplanation = "Lading is een eigenschap die bepaalt hoe een deeltje wordt beïnvloed door een elektrisch of magnetisch veld.", YoutubeId = "0ouf-xbz7_o", ImageUrl = "https://s3-us-west-2.amazonaws.com/leerlevels/slide_pngs/2.png" }
        );
    }
}
