using ForumWebsite.Models.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForumWebsite.Models.Context
{
    public class ForumDbContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;


        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(x => x.AboutMe)
                    .HasMaxLength(255);
            });

            builder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                // Column properties
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.Header)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(x => x.Body)
                    .HasMaxLength(65535)
                    .IsRequired();

                entity.Property(x => x.CreatedTimestamp)
                    .IsRequired();

                entity.Property(x => x.IsClosed)
                    .IsRequired();

                // Column names
                entity.Property(x => x.Id).HasColumnName("Id");
                entity.Property(x => x.Header).HasColumnName("Header");
                entity.Property(x => x.Body).HasColumnName("Body");
                entity.Property(x => x.CreatedTimestamp).HasColumnName("CreatedTimestamp");
                entity.Property(x => x.IsClosed).HasColumnName("IsClosed");
                entity.Property(x => x.UserId).HasColumnName("UserId");

                // Relations
                entity.HasOne(x => x.User)
                    .WithMany(x => x.Posts)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_User_Post");
            });

            builder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                // Column properties
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.Body)
                    .HasMaxLength(65535)
                    .IsRequired();

                entity.Property(x => x.CreatedTimestamp)
                    .IsRequired();

                // Relations
                entity.HasOne(x => x.User)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_User_Comment");

                entity.HasOne(x => x.Post)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(x => x.PostId)
                    .HasConstraintName("FK_Post_Comment");
            });
        }
    }
}
