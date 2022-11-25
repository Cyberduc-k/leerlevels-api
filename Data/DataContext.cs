using Microsoft.EntityFrameworkCore;
using Model;

namespace Data;

public class DataContext : DbContext
{
    private readonly bool _addBasicEntities = true;

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }

    public DbSet<Forum> Forums { get; set; }
    public DbSet<ForumReply> Replies { get; set; }

    public DbSet<Set> Sets { get; set; }
    public DbSet<Target> Targets { get; set; }
    public DbSet<Mcq> Mcqs { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }

    public DbSet<Bookmark> Bookmarks { get; set; }

    public DbSet<TargetProgress> TargetProgress { get; set; }
    public DbSet<McqProgress> McqProgress { get; set; }
    public DbSet<GivenAnswerOption> GivenAnswerOptions { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
        // Database.EnsureCreated();
    }

    public DataContext(DbContextOptions options, bool addBasicEntities) : base(options)
    {
        _addBasicEntities = addBasicEntities;
        // Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>()
            .HasMany(g => g.Users)
            .WithMany(u => u.Groups);

        modelBuilder.Entity<Set>(e => {
            e.HasMany(s => s.Users).WithMany(u => u.Sets);
            e.HasMany(s => s.Targets).WithMany(t => t.Sets);
        });

        modelBuilder.Entity<Bookmark>(e => {
            e.HasKey(b => new { b.UserId, b.ItemId, b.Type });
            e.ToTable("Bookmarks");
        });

        modelBuilder.Entity<GivenAnswerOption>(e => {
            e.HasKey(a => new { a.AnswerId, a.AnswerKind });
            e.ToTable("GivenAnswerOption");
        });

        if (_addBasicEntities) {
            modelBuilder.Entity<User>().HasData(
            new {
                Id = "1",
                Email = "JohnDoe@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                UserName = "JohnD#1",
                Password = $"{Environment.GetEnvironmentVariable("TokenHashBase")!}$AQAAAAEAACcQAAAAEC5ynu4doF+Ndp8v1pvxkxfUseWCAZ6095V/GCQqA4L9A1hi0t/1gBLf8atq8P60Aw==", // "J0hn#001!"
                Role = UserRole.Student,
                LastLogin = DateTime.Parse("2022-10-05 13:27:00"),
                LastDeviceHandle = "11",
                ShareCode = "DTRY-WQER-PIGU-VNSA",
                IsActive = true
            },
            new {
                Id = "2",
                Email = "MarySue@gmail.com",
                FirstName = "Mary",
                LastName = "Sue",
                UserName = "MarySue#22",
                Password = $"{Environment.GetEnvironmentVariable("TokenHashBase")!}$AQAAAAEAACcQAAAAEPvUCXnvR1fic6e98jZnZqyD2GUauqKwWnEVsMu5AGbm1PggvwocdtgxW/IIfeZh8g==", // "M4rySu3san#22!"
                Role = UserRole.Teacher,
                LastLogin = DateTime.Now,
                LastDeviceHandle = "22",
                ShareCode = "RIBN-QWOR-DCPL-AXCU",
                IsActive = true
            },
            new {
                Id = "3",
                Email = "Admin@gmail.com",
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "Admin",
                Password = $"{Environment.GetEnvironmentVariable("TokenHashBase")!}$AQAAAAEAACcQAAAAEKqxLer0zNhevpkLr0sqS4tClT3Gnn77qx+/4FcGAWE/F7AqSPpig8dL7s09znzUzQ==", // "123"
                Role = UserRole.Administrator,
                LastLogin = DateTime.Now,
                LastDeviceHandle = "33",
                ShareCode = "RIBN-QWOR-DCPL-AXCV",
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
}
