using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;

namespace BloggingTool.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EmailAccount> EmailAccount { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleContent> ArticlesContents { get; set; }
        public DbSet<ArticleComment> ArticlesComments { get; set; }
        public DbSet<ArticleLike> ArticleLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // User Model Builder
            modelBuilder.Entity<User>(userEntity => {
                userEntity.Property(user => user.UserId)
                    .ValueGeneratedOnAdd();

                userEntity.Property(user => user.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // EmailAccount Model Builder
            modelBuilder.Entity<EmailAccount>(emailAccountEntity => {
                emailAccountEntity.Property(emailAccount => emailAccount.EmailAccountId)
                    .ValueGeneratedOnAdd();

                emailAccountEntity.Property(EmailAccount => EmailAccount.EmailAddress)
                    .IsRequired() 
                    .HasMaxLength(100);

                emailAccountEntity.HasOne(emailAccount => emailAccount.User)
                    .WithMany(user => user.EmailAccounts)
                    .HasForeignKey(emailAccount => emailAccount.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Blog Model Builder
            modelBuilder.Entity<Blog>(blogEntity  => {
                blogEntity.Property(blog => blog.BlogId)
                    .ValueGeneratedOnAdd();

                blogEntity.Property(blog => blog.BlogTitle)
                    .IsRequired()
                    .HasMaxLength(50);

                blogEntity.Property(blog => blog.BlogAuthor)
                    .IsRequired()
                    .HasMaxLength(50);

                blogEntity.Property(blog => blog.BlogCategory)
                    .IsRequired()
                    .HasMaxLength(50);

                blogEntity.HasOne(blog => blog.EmailAccount)
                    .WithMany(emailAccount => emailAccount.Blogs)
                    .HasForeignKey(blog => blog.EmailAccountId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);    
            });

            // Article Model Builder
            modelBuilder.Entity<Article>(entity => {
                entity.Property(article => article.ArticleId)
                    .ValueGeneratedOnAdd();

                entity.Property(article => article.ArticleTitle)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(article => article.ArticleStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(article => article.CreatedTimestamp)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now', 'localtime')");

                entity.Property(article => article.UpdatedTimestamp)
                    .IsRequired(false);

                entity.Property(article => article.PublishedTimestamp)
                    .IsRequired(false);

                entity.Property(article => article.ArticleViewsCount)
                    .IsRequired(false)
                    .HasDefaultValue(0);

                entity.HasOne(article => article.Blog)
                    .WithMany(blog => blog.Articles)
                    .HasForeignKey(article => article.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        // Article Content Model Builder
            modelBuilder.Entity<ArticleContent>(entity => {
                entity.Property(articleContent => articleContent.ContentId)
                    .ValueGeneratedOnAdd();

                entity.Property(articleContent => articleContent.Content)
                    .IsRequired();

                entity.HasOne(articleContent => articleContent.Article)
                    .WithOne(article => article.ArticleContent)
                    .HasForeignKey<ArticleContent>(articleContent => articleContent.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        // Article Comments Model Builder
            modelBuilder.Entity<ArticleComment>(entity => {
                entity.Property(articleComment => articleComment.CommentId)
                    .ValueGeneratedOnAdd();

                entity.Property(articleComment => articleComment.CommentatorName)
                    .IsRequired();

                entity.Property(articleComment => articleComment.Comment)
                    .IsRequired();


                entity.Property(articleComment => articleComment.CreatedTimestamp)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now', 'localtime')");               

                entity.HasOne(articleComment => articleComment.Article)
                    .WithMany(article => article.ArticleComments)
                    .HasForeignKey(articleComment => articleComment.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        // Article Likes Model Builder
            modelBuilder.Entity<ArticleLike>(entity => {
                entity.Property(articleLike => articleLike.LikeId)
                    .ValueGeneratedOnAdd();

                entity.HasKey(articleLike => articleLike.LikeId);

                entity.HasOne(articleLike => articleLike.Article)
                    .WithMany(article => article.ArticleLikes)
                    .HasForeignKey(articleLike => articleLike.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(articleLike => articleLike.EmailAccount)
                    .WithMany(emailAccount => emailAccount.ArticleLikes)
                    .HasForeignKey(articleLike => articleLike.EmailAccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
