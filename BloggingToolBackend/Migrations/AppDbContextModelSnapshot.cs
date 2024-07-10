﻿// <auto-generated />
using System;
using BloggingTool.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BloggingToolBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("BloggingTool.Models.Article", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ArticleAuthor")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ArticleStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ArticleTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int?>("ArticleViewsCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<int>("BlogId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTimestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("datetime('now', 'localtime')");

                    b.Property<DateTime?>("PublishedTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedTimestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("ArticleId");

                    b.HasIndex("BlogId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("BloggingTool.Models.ArticleComment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CommentatorName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedTimestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("datetime('now', 'localtime')");

                    b.HasKey("CommentId");

                    b.HasIndex("ArticleId");

                    b.ToTable("ArticlesComments");
                });

            modelBuilder.Entity("BloggingTool.Models.ArticleLike", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmailAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsLiked")
                        .HasColumnType("INTEGER");

                    b.HasKey("LikeId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("EmailAccountId");

                    b.ToTable("ArticleLikes");
                });

            modelBuilder.Entity("BloggingTool.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BlogAuthor")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("BlogCategory")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("BlogTitle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("EmailAccountId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BlogId");

                    b.HasIndex("EmailAccountId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("BloggingTool.Models.EmailAccount", b =>
                {
                    b.Property<int>("EmailAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EmailAccountId");

                    b.HasIndex("UserId");

                    b.ToTable("EmailAccounts");
                });

            modelBuilder.Entity("BloggingTool.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BloggingTool.Models.Article", b =>
                {
                    b.HasOne("BloggingTool.Models.Blog", null)
                        .WithMany("Articles")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BloggingTool.Models.ArticleComment", b =>
                {
                    b.HasOne("BloggingTool.Models.Article", null)
                        .WithMany("ArticleComments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BloggingTool.Models.ArticleLike", b =>
                {
                    b.HasOne("BloggingTool.Models.Article", null)
                        .WithMany("ArticleLikes")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloggingTool.Models.EmailAccount", null)
                        .WithMany("ArticleLikes")
                        .HasForeignKey("EmailAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BloggingTool.Models.Blog", b =>
                {
                    b.HasOne("BloggingTool.Models.EmailAccount", null)
                        .WithMany("Blogs")
                        .HasForeignKey("EmailAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BloggingTool.Models.EmailAccount", b =>
                {
                    b.HasOne("BloggingTool.Models.User", null)
                        .WithMany("EmailAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BloggingTool.Models.Article", b =>
                {
                    b.Navigation("ArticleComments");

                    b.Navigation("ArticleLikes");
                });

            modelBuilder.Entity("BloggingTool.Models.Blog", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("BloggingTool.Models.EmailAccount", b =>
                {
                    b.Navigation("ArticleLikes");

                    b.Navigation("Blogs");
                });

            modelBuilder.Entity("BloggingTool.Models.User", b =>
                {
                    b.Navigation("EmailAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
