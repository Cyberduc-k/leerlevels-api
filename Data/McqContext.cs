using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;
public class McqContext : DbContext
{
    public DbSet<Mcq> Mcqs { get; set; }

    public McqContext(DbContextOptions<McqContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mcq>().HasData(
            new { Id = "1", TargetId = "1", QuestionText = "Wat kun je zeggen over de lading van twee voorwerpen die elkaar aantrekken?", Explanation = "Voorwerpen met gelijksoortige lading (beide positief of beide negatief) stoten elkaar af. Voorwerpen met tegengestelde lading (een positief, een negatief) trekken elkaar aan.", AllowRandom = true });
    }
}
