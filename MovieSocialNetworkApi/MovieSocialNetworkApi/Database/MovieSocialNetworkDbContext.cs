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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
