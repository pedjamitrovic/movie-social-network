using Microsoft.EntityFrameworkCore;
using MovieSocialNetworkApi.Entities;

namespace MovieSocialNetworkApi.Database
{
    public class MovieSocialNetworkDbContext : DbContext
    {
        public MovieSocialNetworkDbContext(DbContextOptions<MovieSocialNetworkDbContext> options) : base(options)
        {
        }

        public DbSet<AbstractContent> AbstractContents { get; set; }
        public DbSet<AbstractUser> AbstractUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Relation> Relations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Relation>()
                .ToTable("Relations")
                .HasKey(e => new { e.FollowingId, e.FollowerId });

            modelBuilder.Entity<Relation>()
                .HasOne(e => e.Follower)
                .WithMany(e => e.Following)
                .HasForeignKey(e => e.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Relation>()
                .HasOne(e => e.Following)
                .WithMany(e => e.Followers)
                .HasForeignKey(e => e.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
