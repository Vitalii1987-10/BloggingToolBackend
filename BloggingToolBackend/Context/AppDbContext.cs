using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;

namespace BloggingTool.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Article> Articles { get; set; }
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

                emailAccountEntity.HasOne<User>() 
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

                blogEntity.HasOne<EmailAccount>()
                    .WithMany(emailAccount => emailAccount.Blogs)
                    .HasForeignKey(blog => blog.EmailAccountId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);    
            });

            // Article Model Builder
            modelBuilder.Entity<Article>(articleEntity => {
                articleEntity.Property(article => article.ArticleId)
                    .ValueGeneratedOnAdd();

                articleEntity.Property(article => article.ArticleTitle)
                    .IsRequired()
                    .HasMaxLength(50);

                articleEntity.Property(article => article.ArticleAuthor)
                    .IsRequired()
                    .HasMaxLength(50);

                articleEntity.Property(article => article.ArticleStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                articleEntity.Property(article => article.CreatedTimestamp)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now', 'localtime')");

                articleEntity.Property(article => article.UpdatedTimestamp)
                    .IsRequired(false);

                articleEntity.Property(article => article.PublishedTimestamp)
                    .IsRequired(false);

                articleEntity.Property(article => article.ArticleViewsCount)
                    .IsRequired(false)
                    .HasDefaultValue(0);

                articleEntity.Property(article => article.Content)
                    .IsRequired();

                articleEntity.HasOne<Blog>()
                    .WithMany(blog => blog.Articles)
                    .HasForeignKey(article => article.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        // Article Comments Model Builder
            modelBuilder.Entity<ArticleComment>(commentEntity => {
                commentEntity.Property(articleComment => articleComment.CommentId)
                    .ValueGeneratedOnAdd();

                commentEntity.Property(articleComment => articleComment.CommentatorName)
                    .IsRequired()
                    .HasMaxLength(50);

                commentEntity.Property(articleComment => articleComment.Comment)
                    .IsRequired();


                commentEntity.Property(articleComment => articleComment.CreatedTimestamp)
                    .IsRequired()
                    .HasDefaultValueSql("datetime('now', 'localtime')");               

                commentEntity.HasOne<Article>()
                    .WithMany(article => article.ArticleComments)
                    .HasForeignKey(articleComment => articleComment.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        // Article Likes Model Builder
            modelBuilder.Entity<ArticleLike>(articleLikeEntity => {
                articleLikeEntity.Property(articleLike => articleLike.LikeId)
                    .ValueGeneratedOnAdd();

                articleLikeEntity.HasKey(articleLike => articleLike.LikeId);

                articleLikeEntity.HasOne(articleLike => articleLike.Article)
                    .WithMany(article => article.ArticleLikes)
                    .HasForeignKey(articleLike => articleLike.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                articleLikeEntity.HasOne(articleLike => articleLike.EmailAccount)
                    .WithMany(emailAccount => emailAccount.ArticleLikes)
                    .HasForeignKey(articleLike => articleLike.EmailAccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
