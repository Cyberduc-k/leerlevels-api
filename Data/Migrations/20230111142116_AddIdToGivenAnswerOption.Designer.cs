﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230111142116_AddIdToGivenAnswerOption")]
    partial class AddIdToGivenAnswerOption
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<string>("GroupsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("Model.AnswerOption", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<string>("McqId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("McqId");

                    b.ToTable("AnswerOptions");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Index = 0,
                            IsCorrect = false,
                            McqId = "1",
                            Text = "Beide voorwerpen zijn negatief geladen"
                        },
                        new
                        {
                            Id = "2",
                            Index = 1,
                            IsCorrect = false,
                            McqId = "1",
                            Text = "Beide voorwerpen zijn positief geladen"
                        },
                        new
                        {
                            Id = "3",
                            Index = 2,
                            IsCorrect = false,
                            McqId = "1",
                            Text = "Beide voorwerpen zijn neutraal geladen"
                        },
                        new
                        {
                            Id = "4",
                            Index = 3,
                            IsCorrect = true,
                            McqId = "1",
                            Text = "De lading is tegengesteld"
                        });
                });

            modelBuilder.Entity("Model.Bookmark", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ItemId", "Type");

                    b.ToTable("Bookmarks", (string)null);
                });

            modelBuilder.Entity("Model.Forum", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.ToTable("Forums");
                });

            modelBuilder.Entity("Model.ForumReply", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ForumId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FromId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Upvotes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ForumId");

                    b.HasIndex("FromId");

                    b.ToTable("Replies");
                });

            modelBuilder.Entity("Model.GivenAnswerOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AnswerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AnswerKind")
                        .HasColumnType("int");

                    b.Property<int?>("McqProgressId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("McqProgressId");

                    b.ToTable("GivenAnswerOptions");
                });

            modelBuilder.Entity("Model.Group", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("EducationType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SchoolYear")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            Id = "DRWA-KCMN-PXYB-ZLQU",
                            EducationType = 1,
                            Name = "Inholland",
                            SchoolYear = 6,
                            Subject = "Programming"
                        });
                });

            modelBuilder.Entity("Model.Mcq", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("AllowRandom")
                        .HasColumnType("bit");

                    b.Property<string>("Explanation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TargetId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TargetId");

                    b.ToTable("Mcqs");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            AllowRandom = true,
                            Explanation = "Voorwerpen met gelijksoortige lading (beide positief of beide negatief) stoten elkaar af. Voorwerpen met tegengestelde lading (een positief, een negatief) trekken elkaar aan.",
                            QuestionText = "Wat kun je zeggen over de lading van twee voorwerpen die elkaar aantrekken?",
                            TargetId = "1"
                        });
                });

            modelBuilder.Entity("Model.McqProgress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("McqId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("TargetProgressId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("McqId");

                    b.HasIndex("TargetProgressId");

                    b.HasIndex("UserId");

                    b.ToTable("McqProgress");
                });

            modelBuilder.Entity("Model.Set", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GroupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Model.Target", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TargetExplanation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("YoutubeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Targets");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Description = "Je kan in eigen woorden uitleggen welk effect lading kan hebben.",
                            ImageUrl = "https://s3-us-west-2.amazonaws.com/leerlevels/slide_pngs/2.png",
                            Label = "Lading concept",
                            TargetExplanation = "Lading is een eigenschap die bepaalt hoe een deeltje wordt beïnvloed door een elektrisch of magnetisch veld.",
                            YoutubeId = "0ouf-xbz7_o"
                        });
                });

            modelBuilder.Entity("Model.TargetLink", b =>
                {
                    b.Property<string>("FromId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ToId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FromId", "ToId");

                    b.HasIndex("ToId");

                    b.ToTable("Links", (string)null);
                });

            modelBuilder.Entity("Model.TargetProgress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("TargetId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TargetId");

                    b.HasIndex("UserId");

                    b.ToTable("TargetProgress");
                });

            modelBuilder.Entity("Model.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastDeviceHandle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("ShareCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Email = "JohnDoe@gmail.com",
                            FirstName = "John",
                            IsActive = true,
                            LastDeviceHandle = "11",
                            LastLogin = new DateTime(2022, 10, 5, 13, 27, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Doe",
                            Password = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$AQAAAAEAACcQAAAAEC5ynu4doF+Ndp8v1pvxkxfUseWCAZ6095V/GCQqA4L9A1hi0t/1gBLf8atq8P60Aw==",
                            Role = 0,
                            ShareCode = "DTRY-WQER-PIGU-VNSA",
                            UserName = "JohnD#1"
                        },
                        new
                        {
                            Id = "2",
                            Email = "MarySue@gmail.com",
                            FirstName = "Mary",
                            IsActive = true,
                            LastDeviceHandle = "22",
                            LastLogin = new DateTime(2023, 1, 11, 15, 21, 16, 222, DateTimeKind.Local).AddTicks(2531),
                            LastName = "Sue",
                            Password = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$AQAAAAEAACcQAAAAEPvUCXnvR1fic6e98jZnZqyD2GUauqKwWnEVsMu5AGbm1PggvwocdtgxW/IIfeZh8g==",
                            Role = 1,
                            ShareCode = "RIBN-QWOR-DCPL-AXCU",
                            UserName = "MarySue#22"
                        },
                        new
                        {
                            Id = "3",
                            Email = "Admin@gmail.com",
                            FirstName = "Admin",
                            IsActive = true,
                            LastDeviceHandle = "33",
                            LastLogin = new DateTime(2023, 1, 11, 15, 21, 16, 222, DateTimeKind.Local).AddTicks(2565),
                            LastName = "Admin",
                            Password = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$AQAAAAEAACcQAAAAEKqxLer0zNhevpkLr0sqS4tClT3Gnn77qx+/4FcGAWE/F7AqSPpig8dL7s09znzUzQ==",
                            Role = 2,
                            ShareCode = "RIBN-QWOR-DCPL-AXCV",
                            UserName = "Admin"
                        });
                });

            modelBuilder.Entity("SetTarget", b =>
                {
                    b.Property<string>("SetsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TargetsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SetsId", "TargetsId");

                    b.HasIndex("TargetsId");

                    b.ToTable("SetTarget");
                });

            modelBuilder.Entity("SetUser", b =>
                {
                    b.Property<string>("SetsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SetsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("SetUser");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("Model.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Model.AnswerOption", b =>
                {
                    b.HasOne("Model.Mcq", null)
                        .WithMany("AnswerOptions")
                        .HasForeignKey("McqId");
                });

            modelBuilder.Entity("Model.Bookmark", b =>
                {
                    b.HasOne("Model.User", null)
                        .WithMany("Bookmarks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Model.Forum", b =>
                {
                    b.HasOne("Model.User", "From")
                        .WithMany()
                        .HasForeignKey("FromId");

                    b.Navigation("From");
                });

            modelBuilder.Entity("Model.ForumReply", b =>
                {
                    b.HasOne("Model.Forum", null)
                        .WithMany("Replies")
                        .HasForeignKey("ForumId");

                    b.HasOne("Model.User", "From")
                        .WithMany()
                        .HasForeignKey("FromId");

                    b.Navigation("From");
                });

            modelBuilder.Entity("Model.GivenAnswerOption", b =>
                {
                    b.HasOne("Model.AnswerOption", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.McqProgress", null)
                        .WithMany("Answers")
                        .HasForeignKey("McqProgressId");

                    b.Navigation("Answer");
                });

            modelBuilder.Entity("Model.Mcq", b =>
                {
                    b.HasOne("Model.Target", "Target")
                        .WithMany("Mcqs")
                        .HasForeignKey("TargetId");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("Model.McqProgress", b =>
                {
                    b.HasOne("Model.Mcq", "Mcq")
                        .WithMany()
                        .HasForeignKey("McqId");

                    b.HasOne("Model.TargetProgress", null)
                        .WithMany("Mcqs")
                        .HasForeignKey("TargetProgressId");

                    b.HasOne("Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Mcq");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Model.Set", b =>
                {
                    b.HasOne("Model.Group", "Group")
                        .WithMany("Sets")
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Model.TargetLink", b =>
                {
                    b.HasOne("Model.Target", "From")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.Target", "To")
                        .WithMany()
                        .HasForeignKey("ToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("From");

                    b.Navigation("To");
                });

            modelBuilder.Entity("Model.TargetProgress", b =>
                {
                    b.HasOne("Model.Target", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId");

                    b.HasOne("Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Target");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SetTarget", b =>
                {
                    b.HasOne("Model.Set", null)
                        .WithMany()
                        .HasForeignKey("SetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.Target", null)
                        .WithMany()
                        .HasForeignKey("TargetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SetUser", b =>
                {
                    b.HasOne("Model.Set", null)
                        .WithMany()
                        .HasForeignKey("SetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Model.Forum", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Model.Group", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("Model.Mcq", b =>
                {
                    b.Navigation("AnswerOptions");
                });

            modelBuilder.Entity("Model.McqProgress", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Model.Target", b =>
                {
                    b.Navigation("Mcqs");
                });

            modelBuilder.Entity("Model.TargetProgress", b =>
                {
                    b.Navigation("Mcqs");
                });

            modelBuilder.Entity("Model.User", b =>
                {
                    b.Navigation("Bookmarks");
                });
#pragma warning restore 612, 618
        }
    }
}
