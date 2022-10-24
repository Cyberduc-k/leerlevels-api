using Microsoft.EntityFrameworkCore;
using Model;
using System.Text;

namespace Data;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }

    public DbSet<Forum> Forums { get; set; }
    public DbSet<ForumReply> Replies { get; set; }

    public DbSet<Set> Sets { get; set; }
    public DbSet<Target> Targets { get; set; }
    public DbSet<Mcq> Mcqs { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = Environment.GetEnvironmentVariable("LeerLevelsDatabase")!;

        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany<Group>(u => u.Groups)
                .WithMany(g => g.Users);
                /*.Map(ug =>
                    {
                        ug.MapLeftKey("UserRefId");
                        ug.MapRightKey("GroupRefId");
                        ug.ToTable("UserGroups");
                });*/

        modelBuilder.Entity<User>()
            .HasMany<Set>(u => u.Sets)
                .WithMany(s => s.Users);
                

        modelBuilder.Entity<User>().HasData(
            new {
                Id = Guid.NewGuid().ToString(),
                Email = "JohnDoe@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                UserName = "JohnD#1",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("J0nh#001!")).ToString(),
                Role = UserRole.Student.ToString(),
                LastLogin = DateTime.Parse("2022-10-05 13:27:00"),
                ShareCode = "DTRY-WQER-PIGU-VNSA",
                IsLoggedIn = false,
                IsActive = true
            },
            new {
                Id = Guid.NewGuid().ToString(),
                Email = "MarySue@gmail.com",
                FirstName = "Mary",
                LastName = "Sue",
                UserName = "MarySue#22",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("M4rySu3san#22!")).ToString(),
                Role = UserRole.Teacher.ToString(),
                LastLogin = DateTime.Now,
                ShareCode = "RIBN-QWOR-DCPL-AXCU",
                IsLoggedIn = false,
                IsActive = true
            }
        );

        modelBuilder.Entity<Target>().HasData(
            new {
                Id = "1",
                Label = "Lading concept",
                Description = "Je kan in eigen woorden uitleggen welk effect lading kan hebben.",
                TargetExplanation = "Lading is een eigenschap die bepaalt hoe een deeltje wordt beïnvloed door een elektrisch of magnetisch veld.",
                YoutubeId = "0ouf-xbz7_o",
                ImageUrl = "https://s3-us-west-2.amazonaws.com/leerlevels/slide_pngs/2.png"
            }
        );

        modelBuilder.Entity<Group>().HasData(
            new { Id = "DRWA-KCMN-PXYB-ZLQU", Name = "Inholland", Subject = "Programming", EducationType = EducationType.Mavo, SchoolYear = SchoolYear.Seven }
        );

        modelBuilder.Entity<Mcq>().HasData(
            new {
                Id = "1",
                TargetId = "1",
                QuestionText = "Wat kun je zeggen over de lading van twee voorwerpen die elkaar aantrekken?",
                Explanation = "Voorwerpen met gelijksoortige lading (beide positief of beide negatief) stoten elkaar af. Voorwerpen met tegengestelde lading (een positief, een negatief) trekken elkaar aan.",
                AllowRandom = true
            }
        );

        modelBuilder.Entity<AnswerOption>().HasData(
            new { Id = "1", McqId = "1", Index = 0, IsCorrect = false, Text = "Beide voorwerpen zijn negatief geladen" },
            new { Id = "2", McqId = "1", Index = 1, IsCorrect = false, Text = "Beide voorwerpen zijn positief geladen" },
            new { Id = "3", McqId = "1", Index = 2, IsCorrect = false, Text = "Beide voorwerpen zijn neutraal geladen" },
            new { Id = "4", McqId = "1", Index = 3, IsCorrect = true, Text = "De lading is tegengesteld" }
        );
    }
}
